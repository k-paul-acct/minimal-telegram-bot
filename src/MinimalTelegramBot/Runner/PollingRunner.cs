using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Runner;

internal static partial class PollingRunner
{
    public static async Task<IHost> StartPolling(this BotApplication app, UpdateHandler updateHandler, CancellationToken ct)
    {
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var botClient = app.Services.GetRequiredService<ITelegramBotClient>();
        var logger = loggerFactory.CreateLogger(nameof(Runner));

        await app._host.StartAsync(ct);

        botClient.StartReceiving(UpdateHandler, PollingErrorHandler, app._options.ReceiverOptions, ct);

        return app._host;

        Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            Task.Run(() => updateHandler.Handle(update), cancellationToken);
            return Task.CompletedTask;
        }

        Task PollingErrorHandler(ITelegramBotClient client, Exception ex, CancellationToken cancellationToken)
        {
            Log.PollingError(logger, ex.Message);
            return Task.CompletedTask;
        }
    }

    private static partial class Log
    {
        [LoggerMessage(500, LogLevel.Error, "Polling error: {message}", EventName = nameof(PollingError))]
        public static partial void PollingError(ILogger logger, string message);
    }
}
