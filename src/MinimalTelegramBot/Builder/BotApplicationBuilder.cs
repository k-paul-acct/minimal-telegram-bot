using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinimalTelegramBot.Settings;
using Telegram.Bot;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     Provides a builder for configuring and creating a BotApplication instance.
/// </summary>
public sealed class BotApplicationBuilder : IHostApplicationBuilder
{
    private readonly HostApplicationBuilder _hostBuilder;

    internal readonly BotApplicationBuilderOptions _options;

    internal BotApplicationBuilder(BotApplicationBuilderOptions options)
    {
        _hostBuilder = Host.CreateApplicationBuilder(options.HostApplicationBuilderSettings);
        _options = options;
        TrySetBotToken();
    }

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.HostApplicationBuilder.Configuration"/>
    public IConfigurationManager Configuration => _hostBuilder.Configuration;

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.HostApplicationBuilder.Services"/>
    public IServiceCollection Services => _hostBuilder.Services;

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.HostApplicationBuilder.Logging"/>
    public ILoggingBuilder Logging => _hostBuilder.Logging;

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.HostApplicationBuilder.Environment"/>
    public IHostEnvironment Environment => _hostBuilder.Environment;

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.HostApplicationBuilder.Metrics"/>
    public IMetricsBuilder Metrics => _hostBuilder.Metrics;

    IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_hostBuilder).Properties;

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.HostApplicationBuilder.ConfigureContainer{TContainerBuilder}"/>
    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null)
        where TContainerBuilder : notnull
    {
        _hostBuilder.ConfigureContainer(factory, configure);
    }

    /// <summary>
    ///     Builds a new instance of <see cref="BotApplication"/>.
    /// </summary>
    /// <returns>A new instance of <see cref="BotApplication"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the bot token is not configured.</exception>
    public BotApplication Build()
    {
        TrySetBotToken();

        if (_options.Token is null)
        {
            throw new InvalidOperationException($"Cannot build a {nameof(BotApplication)} without a bot token configured");
        }

        var telegramBotClientOptions = new TelegramBotClientOptions(_options.Token);
        _options.TelegramBotClientOptionsConfigure?.Invoke(telegramBotClientOptions);

        var client = new TelegramBotClient(telegramBotClientOptions);

        AddDefaultServices(client);

        var host = _hostBuilder.Build();

        return new BotApplication(host, client, new BotApplicationOptions(_options, _options.Token));
    }

    private void AddDefaultServices(ITelegramBotClient client)
    {
        Services.TryAddSingleton(client);
        Services.TryAddSingleton<IBotRequestContextAccessor, BotRequestContextAccessor>();
    }

    private void TrySetBotToken()
    {
        _options.Token = Configuration["TelegramBotToken"] ?? Configuration["BotToken"] ?? Configuration["Token"];
    }
}
