using Microsoft.Extensions.DependencyInjection;
using MinimalTelegramBot.Client;
using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     ConfigurationExtensions.
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    ///     Configures the bot token for the <see cref="Telegram.Bot.ITelegramBotClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the configuration to.</param>
    /// <param name="token">The bot token.</param>
    /// <returns>The current instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection ConfigureBotToken(this IServiceCollection services, string token)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(token);

        services.Configure<TelegramBotClientOptions>(options => options.Token = token);

        return services;
    }

    /// <summary>
    ///     Configures the <see cref="ReceiverOptions"/> for the <see cref="Telegram.Bot.ITelegramBotClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the configuration to.</param>
    /// <param name="configure">The action to configure the <see cref="ReceiverOptions"/>.</param>
    /// <returns>The current instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection ConfigureReceiverOptions(this IServiceCollection services, Action<ReceiverOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.Configure(configure);

        return services;
    }

    /// <summary>
    ///     Configures the <see cref="TelegramBotClientOptions"/> for the <see cref="Telegram.Bot.ITelegramBotClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the configuration to.</param>
    /// <param name="configure">The action to configure the <see cref="TelegramBotClientOptions"/>.</param>
    /// <returns>The current instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection ConfigureTelegramBotClientOptions(this IServiceCollection services, Action<TelegramBotClientOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.Configure(configure);

        return services;
    }
}
