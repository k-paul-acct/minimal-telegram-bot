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
}
