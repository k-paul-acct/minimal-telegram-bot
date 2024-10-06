using Telegram.Bot;
using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     BotApplicationBuilderExtensions.
/// </summary>
public static class BotApplicationBuilderExtensions
{
    /// <summary>
    ///     Sets the bot token for the application.
    /// </summary>
    /// <param name="builder">The bot application builder.</param>
    /// <param name="token">The bot token.</param>
    /// <returns>The current instance of <see cref="BotApplicationBuilder"/>.</returns>
    public static BotApplicationBuilder SetBotToken(this BotApplicationBuilder builder, string token)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(token);

        builder._options.Token = token;

        return builder;
    }

    /// <summary>
    ///     Configures the receiver options for the bot.
    /// </summary>
    /// <param name="builder">The bot application builder.</param>
    /// <param name="configure">The action to configure the receiver options.</param>
    /// <returns>The current instance of <see cref="BotApplicationBuilder"/>.</returns>
    public static BotApplicationBuilder ConfigureReceiverOptions(this BotApplicationBuilder builder, Action<ReceiverOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        configure(builder._options.ReceiverOptions);

        return builder;
    }

    /// <summary>
    ///     Configures the Telegram bot client options.
    /// </summary>
    /// <param name="builder">The bot application builder.</param>
    /// <param name="configure">The action to configure the Telegram bot client options.</param>
    /// <returns>The current instance of <see cref="BotApplicationBuilder"/>.</returns>
    public static BotApplicationBuilder ConfigureTelegramBotClientOptions(this BotApplicationBuilder builder, Action<TelegramBotClientOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder._options.TelegramBotClientOptionsConfigure = configure;

        return builder;
    }
}
