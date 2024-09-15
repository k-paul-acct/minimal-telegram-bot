using Telegram.Bot;

namespace MinimalTelegramBot.Runner;

internal static class FeatureHandler
{
    public static async Task HandleStartupFeatures(this BotApplication app)
    {
        IBotApplicationBuilder builder = app;
        await HandleDeleteWebhookOnStartupFeature(app, builder);
    }

    public static async Task HandleShutdownFeatures(this BotApplication app)
    {
        IBotApplicationBuilder builder = app;
        await HandleDeleteWebhookOnShutdownFeature(app, builder);
    }

    private static Task HandleDeleteWebhookOnStartupFeature(BotApplication app, IBotApplicationBuilder builder)
    {
        return builder.Properties.ContainsKey("__DeleteWebhookOnStartup")
            ? DeleteWebhook(app._client, app._options.ReceiverOptions.DropPendingUpdates)
            : Task.CompletedTask;
    }

    private static Task HandleDeleteWebhookOnShutdownFeature(BotApplication app, IBotApplicationBuilder builder)
    {
        return builder.Properties.ContainsKey("__DeleteWebhookOnShutdown")
            ? DeleteWebhook(app._client, app._options.ReceiverOptions.DropPendingUpdates)
            : Task.CompletedTask;
    }

    private static Task DeleteWebhook(ITelegramBotClient client, bool dropPendingUpdates)
    {
        return client.DeleteWebhookAsync(dropPendingUpdates);
    }
}
