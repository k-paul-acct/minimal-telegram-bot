using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinimalTelegramBot.Settings;
using Telegram.Bot;

namespace MinimalTelegramBot.Builder;

public sealed class BotApplicationBuilder : IHostApplicationBuilder
{
    private readonly HostApplicationBuilder _hostBuilder;

    internal readonly BotApplicationBuilderOptions _options;

    internal BotApplicationBuilder(BotApplicationBuilderOptions options)
    {
        _hostBuilder = Host.CreateApplicationBuilder(options.HostApplicationBuilderSettings);
        _options = options;
    }

    public IConfigurationManager Configuration => _hostBuilder.Configuration;
    public IServiceCollection Services => _hostBuilder.Services;
    public ILoggingBuilder Logging => _hostBuilder.Logging;
    public IHostEnvironment Environment => _hostBuilder.Environment;
    public IMetricsBuilder Metrics => _hostBuilder.Metrics;

    IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_hostBuilder).Properties;

    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null)
        where TContainerBuilder : notnull
    {
        _hostBuilder.ConfigureContainer(factory, configure);
    }

    public BotApplication Build()
    {
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
}
