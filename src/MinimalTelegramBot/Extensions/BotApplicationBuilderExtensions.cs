using MinimalTelegramBot.Settings;

namespace MinimalTelegramBot.Extensions;

public static class BotApplicationBuilderExtensions
{
    public static IBotApplicationBuilder UsePolling(this IBotApplicationBuilder app, bool deleteWebhook = false)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.Properties.ContainsKey("__WebhookEnabled"))
        {
            throw new InvalidOperationException("Cannot use polling because webhook already used");
        }

        app.Properties.TryAdd("__PollingEnabled", true);
        app.Properties.TryAdd("__DeleteWebhook", deleteWebhook);

        return app;
    }

    public static IBotApplicationBuilder UseWebhook(this IBotApplicationBuilder app, WebhookOptions options)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(options);

        if (app.Properties.ContainsKey("__PollingEnabled"))
        {
            throw new InvalidOperationException("Cannot use webhook because polling already used");
        }

        app.Properties.TryAdd("__WebhookEnabled", options);

        return app;
    }
}
