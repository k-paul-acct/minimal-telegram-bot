using MinimalTelegramBot.Logging;
using MinimalTelegramBot.Server;
using Telegram.Bot;

namespace MinimalTelegramBot.Runner;

internal static class BotApplicationRunner
{
    public static async Task RunAsync(BotApplication app, CancellationToken cancellationToken)
    {
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var infrastructureLogger = new InfrastructureLogger(loggerFactory);

        IBotApplicationBuilder builder = app;
        var pipeline = builder.Build();
        var isWebhook = builder.Properties.ContainsKey("__WebhookEnabled");
        var updateServer = new UpdateServer(app.Services, pipeline, builder.Properties, infrastructureLogger);

        await app.HandleFeatures();

        var hostTask = isWebhook ? app.StartWebhook(updateServer) : app.StartPolling(updateServer, infrastructureLogger);
        var host = await hostTask;

        var botInfo = await app._client.GetBotStartupInfo();

        infrastructureLogger.GettingUpdateStarted(isWebhook, botInfo.Username, botInfo.FullName, botInfo.Id);

        await host.WaitForShutdownAsync(cancellationToken);
    }

    private static async Task<BotStartupInfo> GetBotStartupInfo(this ITelegramBotClient client)
    {
        var bot = await client.GetMeAsync();
        return new BotStartupInfo
        {
            Id = bot.Id,
            FirstName = bot.FirstName,
            LastName = bot.LastName,
            Username = bot.Username ?? "N/A",
        };
    }
}
