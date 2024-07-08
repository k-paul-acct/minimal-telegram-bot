using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotFramework.Handling;
using TelegramBotFramework.Localization.Abstractions;
using TelegramBotFramework.Pipeline;
using TelegramBotFramework.Services;
using TelegramBotFramework.Settings;
using TelegramBotFramework.StateMachine.Abstractions;

namespace TelegramBotFramework;

public class BotApplication : IBotApplicationBuilder, IHandlerBuilder
{
    private readonly TelegramBotClient _client;
    private readonly IHandlerBuilder _handlerBuilder;
    private readonly BotApplicationOptions _options;
    private readonly PipelineBuilder _pipelineBuilder = new();
    private Func<BotRequestContext, Task>? _pipeline;

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

    public IDictionary<string, object?> Properties => _pipelineBuilder.Properties;

    public IBotApplicationBuilder Use(Func<Func<BotRequestContext, Task>, Func<BotRequestContext, Task>> pipe)
    {
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
        return _handlerBuilder.Handle(handlerDelegate);
    }

    public Handler Handle(Func<BotRequestContext, Task> func)
    {
        return _handlerBuilder.Handle(func);
    }

    public Handler? TryResolveHandler(BotRequestContext ctx)
    {
        return _handlerBuilder.TryResolveHandler(ctx);
    }

    internal void RunBot()
    {
        _client.StartReceiving(UpdateHandler, PollingErrorHandler, _options.ReceiverOptions);
        using var scope = Host.Services.CreateScope();
        var botInitService = scope.ServiceProvider.GetRequiredService<BotInitService>();
        botInitService.InitBot().GetAwaiter().GetResult();
    }

    public static BotApplicationBuilder CreateBuilder()
    {
        return new BotApplicationBuilder(args: null);
    }

    public static BotApplicationBuilder CreateBuilder(string[] args)
    {
        return new BotApplicationBuilder(args);
    }

    public static BotApplicationBuilder CreateBuilder(BotApplicationBuilderOptions options)
    {
        return new BotApplicationBuilder(options);
    }

    public void Run()
    {
        if (Properties.ContainsKey("CallbackAutoAnsweringAdded"))
        {
            this.UsePipe<CallbackAutoAnsweringPipe>();
        }

        this.UsePipe<HandlerResolverPipe>();
        BotApplicationRunner.Run(this);
    }

    private Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => HandleUpdateInBackground(client, update), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task HandleUpdateInBackground(ITelegramBotClient client, Update update)
    {
        using var scope = Host.Services.CreateScope();
        
        var context = new BotRequestContext();
        var contextAccessor = scope.ServiceProvider.GetRequiredService<IBotRequestContextAccessor>();
        contextAccessor.BotRequestContext = context;

        var chatId = update.Message?.Chat.Id ?? update.CallbackQuery?.Message?.Chat.Id ?? 0;
        if (chatId == 0)
        {
            return;
        }

        var messageText = update.Message?.Text;
        var callbackData = update.CallbackQuery?.Data;

        var stateMachine = scope.ServiceProvider.GetRequiredService<IStateMachine>();
        var localeService = scope.ServiceProvider.GetService<IUserLocaleService>();
        var locale = localeService is null
            ? null
            : await localeService.GetFromRepositoryOrUpdateWithProviderAsync(chatId);

        context.Client = client;
        context.Update = update;
        context.ChatId = chatId;
        context.MessageText = messageText;
        context.CallbackData = callbackData;
        context.UserLocale = locale;
        context.Services = scope.ServiceProvider;
        context.StateMachine = stateMachine;
        context.UserState = stateMachine.GetState(chatId);

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
}