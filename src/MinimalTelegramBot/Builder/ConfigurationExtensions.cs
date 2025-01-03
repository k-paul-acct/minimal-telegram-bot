using Microsoft.Extensions.DependencyInjection;
using MinimalTelegramBot.Client;
using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     ConfigurationExtensions.
/// </summary>
public static class ConfigurationExtensions
{
    // TODO: Docs.
    /// <summary>
    ///     Sets the bot token for the application.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="token">The bot token.</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureBotToken(this IServiceCollection services, string token)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(token);

        services.Configure<TelegramBotClientOptions>(options => options.Token = token);

        return services;
    }

    /// <summary>
    ///     Configures the receiver options for the bot.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure">The action to configure the receiver options.</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureReceiverOptions(this IServiceCollection services, Action<ReceiverOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.Configure(configure);

        return services;
    }

    /// <summary>
    ///     Configures the Telegram bot client options.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure">The action to configure the Telegram bot client options.</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureTelegramBotClientOptions(this IServiceCollection services, Action<TelegramBotClientOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.Configure(configure);

        return services;
    }
}
