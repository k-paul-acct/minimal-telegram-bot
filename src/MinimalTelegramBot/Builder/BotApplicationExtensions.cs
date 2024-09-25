using MinimalTelegramBot.Settings;

namespace MinimalTelegramBot.Builder;

public static class BotApplicationExtensions
{
    public static IPollingBuilder UsePolling(this IBotApplicationBuilder app, bool deleteWebhook = false)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.Properties.ContainsKey("__WebhookEnabled"))
        {
            throw new InvalidOperationException("Cannot use polling because webhook already used");
        }

        if (deleteWebhook)
        {
            app.Properties.TryAdd("__DeleteWebhookOnStartup", new object());
        }

        var pollingBuilder = new PollingBuilder();

        app.Properties.TryAdd("__PollingEnabled", pollingBuilder);

        return pollingBuilder;
    }

    public static IWebhookBuilder UseWebhook(this IBotApplicationBuilder app, WebhookOptions options)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(options);

        if (app.Properties.ContainsKey("__PollingEnabled"))
        {
            throw new InvalidOperationException("Cannot use webhook because polling already used");
        }

        var webhookBuilder = new WebhookBuilder(options);

        app.Properties.TryAdd("__WebhookEnabled", webhookBuilder);

        return webhookBuilder;
    }

    public static IBotApplicationBuilder UseCallbackAutoAnswering(this IBotApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.Properties["__CallbackAutoAnsweringPipeAdded"] = true;
        return app;
    }
}
