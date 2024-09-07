using Telegram.Bot;

namespace MinimalTelegramBot.Runner;

internal static partial class BotApplicationRunner
{
    public static async Task RunAsync(BotApplication app, CancellationToken cancellationToken)
    {
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("MinimalTelegramBot.Runner");

        IBotApplicationBuilder builder = app;
        var pipeline = builder.Build();
        var updateProcessor = new UpdateHandler(builder.Services, pipeline);
        var isWebhook = builder.Properties.ContainsKey("__WebhookEnabled");

        await app.HandleDeleteWebhookFeature(cancellationToken);
        var host = await (isWebhook ? app.StartWebhook(updateProcessor, cancellationToken) : app.StartPolling(updateProcessor, cancellationToken));

        var gettingUpdatesVia = isWebhook ? "Webhook" : "Polling";
        var client = app.Services.GetRequiredService<ITelegramBotClient>();
        var info = await GetBotStartupInfo(client, cancellationToken);
        Log.GettingUpdateStarted(logger, gettingUpdatesVia, info.BotUsername, info.BotFullName, info.BotId);

        await host.WaitForShutdownAsync(cancellationToken);
    }

    private static async Task<BotStartupInfo> GetBotStartupInfo(ITelegramBotClient client, CancellationToken cancellationToken)
    {
        var bot = await client.GetMeAsync(cancellationToken);
        return new BotStartupInfo
        {
            BotId = bot.Id,
            BotUsername = bot.Username ?? "N/A",
            BotFirstName = bot.FirstName,
            BotLastName = bot.LastName,
        };
    }

    private static async Task HandleDeleteWebhookFeature(this BotApplication app, CancellationToken cancellationToken)
    {
        IBotApplicationBuilder builder = app;
        builder.Properties.TryGetValue("__DeleteWebhook", out var deleteWebhook);

        if (deleteWebhook is true)
        {
            var dropPendingUpdates = app._options.ReceiverOptions?.DropPendingUpdates ?? false;
            await app._client.DeleteWebhookAsync(dropPendingUpdates, cancellationToken);
        }
    }

    private static partial class Log
    {
        [LoggerMessage(0, LogLevel.Information, "Getting updates via {gettingUpdatesVia} started for bot @{botUsername} ({botFullName}) with ID = {botId}", EventName = nameof(GettingUpdateStarted))]
        public static partial void GettingUpdateStarted(ILogger logger, string gettingUpdatesVia, string botUsername, string botFullName, long botId);
    }
}
