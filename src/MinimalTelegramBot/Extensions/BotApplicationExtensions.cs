using MinimalTelegramBot.Settings;

namespace MinimalTelegramBot.Extensions;

public static class BotApplicationExtensions
{
    public static void UsePolling(this BotApplication app)
    {
        if (app.Properties.ContainsKey("WebhooksEnabled"))
        {
            throw new Exception("Cannot use polling because webhook already used");
        }

        app.Properties.TryAdd("PollingEnabled", true);
    }
    
    public static void UseWebhook(this BotApplication app, WebhookOptions options)
    {
        if (app.Properties.ContainsKey("PollingEnabled"))
        {
            throw new Exception("Cannot use webhook because polling already used");
        }

        app.Properties.TryAdd("WebhooksEnabled", true);
        app.Options.WebhookOptions = options;
    }
}