using MinimalTelegramBot.Settings;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     BotApplicationExtensions.
/// </summary>
public static class BotApplicationExtensions
{
    /// <summary>
    ///     Configures the bot application to use polling method for getting updates.
    /// </summary>
    /// <param name="app">The bot application builder.</param>
    /// <param name="deleteWebhook">Indicates whether to delete the webhook on startup.</param>
    /// <returns>A polling builder instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if webhook is already enabled.</exception>
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

    /// <summary>
    ///     Configures the bot application to use a webhook method for getting updates.
    /// </summary>
    /// <param name="app">The bot application builder.</param>
    /// <param name="options">The webhook options.</param>
    /// <returns>A webhook builder instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if polling is already enabled.</exception>
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

    /// <summary>
    ///     Configures the bot to automatically answer callback queries.
    /// </summary>
    /// <param name="app">The bot application builder.</param>
    /// <returns>The current instance of <see cref="IBotApplicationBuilder"/>.</returns>
    public static IBotApplicationBuilder UseCallbackAutoAnswering(this IBotApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.Properties["__CallbackAutoAnsweringPipeAdded"] = true;
        return app;
    }
}
