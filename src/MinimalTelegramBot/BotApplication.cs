using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.Services;
using MinimalTelegramBot.Settings;
using MinimalTelegramBot.StateMachine.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot;

public sealed class BotApplication : IBotApplicationBuilder, IHandlerBuilder, IHost
{
    private readonly IHandlerBuilder _handlerBuilder;
    private readonly PipelineBuilder _pipelineBuilder = new();
    private Func<BotRequestContext, Task>? _pipeline;

    internal readonly TelegramBotClient _client;
    internal readonly BotApplicationOptions _options;

    internal BotApplication(IHost host, TelegramBotClient client, BotApplicationOptions options,
        IHandlerBuilder handlerBuilder)
    {
        Host = host;
        _client = client;
        _options = options;
        _handlerBuilder = handlerBuilder;

        UseDefaultOuterPipes();
    }

    public IHost Host { get; }

    Task IHost.StartAsync(CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    Task IHost.StopAsync(CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public IServiceProvider Services => Host.Services;

    IDictionary<string, object?> IBotApplicationBuilder.Properties => _pipelineBuilder.Properties;

    public IBotApplicationBuilder Use(Func<Func<BotRequestContext, Task>, Func<BotRequestContext, Task>> pipe)
    {
        ArgumentNullException.ThrowIfNull(pipe);

        _pipelineBuilder.Use(pipe);
        return this;
    }

    Func<BotRequestContext, Task> IBotApplicationBuilder.Build()
    {
        _pipeline = _pipelineBuilder.Build();
        return _pipeline;
    }

    public Handler Handle(Delegate handlerDelegate)
    {
        ArgumentNullException.ThrowIfNull(handlerDelegate);

        return _handlerBuilder.Handle(handlerDelegate);
    }

    public Handler Handle(Func<BotRequestContext, Task> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return _handlerBuilder.Handle(func);
    }

    ValueTask<Handler?> IHandlerBuilder.TryResolveHandler(BotRequestFilterContext ctx)
    {
        return _handlerBuilder.TryResolveHandler(ctx);
    }

    internal async Task InitBot(bool isWebhook, CancellationToken cancellationToken)
    {
        using var scope = Host.Services.CreateScope();
        var botInitService = scope.ServiceProvider.GetRequiredService<BotInitService>();
        await botInitService.InitBot(isWebhook, cancellationToken);
    }

    public static BotApplicationBuilder CreateBuilder()
    {
        return CreateBuilder(new BotApplicationBuilderOptions());
    }

    public static BotApplicationBuilder CreateBuilder(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        return CreateBuilder(new BotApplicationBuilderOptions
        {
            Args = args,
            HostApplicationBuilderSettings = new HostApplicationBuilderSettings
            {
                Args = args,
            },
        });
    }

    public static BotApplicationBuilder CreateBuilder(HostApplicationBuilderSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return CreateBuilder(new BotApplicationBuilderOptions
        {
            Args = settings.Args,
            HostApplicationBuilderSettings = settings,
        });
    }

    private static BotApplicationBuilder CreateBuilder(BotApplicationBuilderOptions options)
    {
        return new BotApplicationBuilder(options);
    }

    public void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (_pipelineBuilder.Properties.ContainsKey("__CallbackAutoAnsweringAdded"))
        {
            this.UsePipe<CallbackAutoAnsweringPipe>();
        }

        this.UsePipe<HandlerResolverPipe>();
        await BotApplicationRunner.RunAsync(this, cancellationToken);
    }

    internal void StartPolling()
    {
        _client.StartReceiving(UpdateHandler, PollingErrorHandler, _options.ReceiverOptions);
    }

    private Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => HandleUpdateInBackground(client, update), cancellationToken);
        return Task.CompletedTask;
    }

    internal async Task HandleUpdateInBackground(ITelegramBotClient client, Update update)
    {
        using var scope = Host.Services.CreateScope();

        var context = new BotRequestContext();
        var contextAccessor = scope.ServiceProvider.GetRequiredService<IBotRequestContextAccessor>();

        contextAccessor.BotRequestContext = context;

        var chatId = update.Message?.Chat.Id ??
                     update.CallbackQuery?.Message?.Chat.Id ??
                     update.EditedMessage?.Chat.Id ??
                     update.ChannelPost?.Chat.Id ??
                     update.EditedChannelPost?.Chat.Id ??
                     update.MessageReaction?.Chat.Id ??
                     update.MessageReactionCount?.Chat.Id ??
                     update.ChatBoost?.Chat.Id ??
                     update.RemovedChatBoost?.Chat.Id ??
                     0;

        var messageText = update.Message?.Text;
        var callbackData = update.CallbackQuery?.Data;

        context.Client = client;
        context.Update = update;
        context.ChatId = chatId;
        context.MessageText = messageText;
        context.CallbackData = callbackData;
        context.Services = scope.ServiceProvider;

        var localeProvider = scope.ServiceProvider.GetService<IUserLocaleProvider>();
        if (localeProvider is not null)
        {
            var locale = await localeProvider.GetUserLocaleAsync(context.ChatId);
            context.UserLocale = locale;
        }

        var stateMachine = scope.ServiceProvider.GetService<IStateMachine>();
        if (stateMachine is not null)
        {
            var state = stateMachine.GetState(context.ChatId);
            context.UserState = state;
        }

        await _pipeline!(context);
    }

    private Task PollingErrorHandler(ITelegramBotClient client, Exception e, CancellationToken cancellationToken)
    {
        using var scope = Host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BotApplication>>();
        logger.LogError(500, e, "Bot error: {Error}", e.Message);
        return Task.CompletedTask;
    }

    private void UseDefaultOuterPipes()
    {
        this.UsePipe<ExceptionHandlerPipe>();
        this.UsePipe<UpdateLoggerPipe>();
    }

    public void Dispose()
    {
        Host.Dispose();
    }
}
