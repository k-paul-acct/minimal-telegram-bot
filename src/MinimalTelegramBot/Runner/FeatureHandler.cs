using Telegram.Bot;

namespace MinimalTelegramBot.Runner;

internal static class FeatureHandler
{
    public static async Task HandleFeatures(this BotApplication app)
    {
        await HandleDeleteWebhookFeature(app);
    }

    private static async Task HandleDeleteWebhookFeature(BotApplication app)
    {
        IBotApplicationBuilder builder = app;
        builder.Properties.TryGetValue("__DeleteWebhook", out var deleteWebhook);

        if (deleteWebhook is true)
        {
            await app._client.DeleteWebhookAsync(app._options.ReceiverOptions.DropPendingUpdates);
        }
    }
}
