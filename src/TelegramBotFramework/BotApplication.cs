using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotFramework.Commands;
using TelegramBotFramework.Extensions;
using TelegramBotFramework.Pipeline;
using TelegramBotFramework.Pipeline.Default;
using TelegramBotFramework.Services;

namespace TelegramBotFramework;

public class BotApplication : IBotApplicationBuilder, IUpdateHandlerBuilder
{
    private readonly TelegramBotClient _client;
    private readonly BotApplicationOptions _options;
    private readonly PipelineBuilder _pipelineBuilder = new();
    private BotRequestDelegate? _pipeline;

    internal BotApplication(IHost host, TelegramBotClient client, BotApplicationOptions options)
    {
        Host = host;
        _client = client;
        _options = options;

        UseDefaultPipes();
    }

    public IHost Host { get; }

    public IBotApplicationBuilder Use(Func<BotRequestDelegate, BotRequestDelegate> pipe)
    {
        _pipelineBuilder.Use(pipe);
        return this;
    }

    BotRequestDelegate IBotApplicationBuilder.Build()
    {
        UseCommandPipe();
        _pipeline = _pipelineBuilder.Build();
        return _pipeline;
    }

    internal void RunBot()
    {
        _client.StartReceiving(
            UpdateHandler,
            PollingErrorHandler,
            _options.ReceiverOptions);

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
        BotApplicationRunner.Run(this);
    }

    private void UseCommandPipe()
    {
        _pipelineBuilder.Use(async (ctx, next) =>
        {
            var text = ctx.Update.Message?.Text;
            if (text is null) return;
            var command = ctx.ServiceProvider.GetKeyedService<ICommand>(text);
            if (command is not null)
            {
                await command.ExecuteAsync(ctx);
            }

            await next(ctx);
        });
    }

    private Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => HandleUpdateInBackground(client, update), cancellationToken);
        return Task.CompletedTask;
    }

    private async Task HandleUpdateInBackground(ITelegramBotClient client, Update update)
    {
        using var scope = Host.Services.CreateScope();

        var ctx = new BotRequestContext
        {
            Client = client,
            Update = update,
            ServiceProvider = scope.ServiceProvider,
        };

        await _pipeline!(ctx);
    }

    private Task PollingErrorHandler(ITelegramBotClient client, Exception e, CancellationToken cancellationToken)
    {
        using var scope = Host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BotApplication>>();
        logger.LogError(e, "Bot error: {Error}", e.Message);

        return Task.CompletedTask;
    }

    private void UseDefaultPipes()
    {
        this.UsePipe<ExceptionHandlerPipe>();
        this.UsePipe<UpdateLoggerPipe>();
    }
}