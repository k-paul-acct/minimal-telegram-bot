using MinimalTelegramBot.Settings;

namespace MinimalTelegramBot.Extensions;

public static class BotApplicationExtensions
{
    public static void UsePolling(this BotApplication app, bool deleteWebhook = false)
    {
        ArgumentNullException.ThrowIfNull(app);

        IBotApplicationBuilder appBuilder = app;

        if (appBuilder.Properties.ContainsKey("__WebhookEnabled"))
        {
            throw new Exception("Cannot use polling because webhook already used");
        }

        appBuilder.Properties.TryAdd("__PollingEnabled", true);
        appBuilder.Properties.TryAdd("__DeleteWebhook", deleteWebhook);
    }

    public static void UseWebhook(this BotApplication app, WebhookOptions options)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(options);

        IBotApplicationBuilder appBuilder = app;

        if (appBuilder.Properties.ContainsKey("__PollingEnabled"))
        {
            throw new Exception("Cannot use webhook because polling already used");
        }

        appBuilder.Properties.TryAdd("__WebhookEnabled", true);
        app._options.WebhookOptions = options;
    }
}