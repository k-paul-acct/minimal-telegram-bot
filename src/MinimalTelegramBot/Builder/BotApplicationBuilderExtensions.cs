using MinimalTelegramBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Builder;

public static class BotApplicationBuilderExtensions
{
    public static BotApplicationBuilder SetBotToken(this BotApplicationBuilder builder, string token)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(token);

        builder._options.Token = token;

        return builder;
    }

    public static BotApplicationBuilder ConfigureReceiverOptions(this BotApplicationBuilder builder, Action<ReceiverOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        configure(builder._options.ReceiverOptions);

        return builder;
    }

    public static BotApplicationBuilder ConfigureTelegramBotClientOptions(this BotApplicationBuilder builder, Action<TelegramBotClientOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder._options.TelegramBotClientOptionsConfigure = configure;

        return builder;
    }

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
