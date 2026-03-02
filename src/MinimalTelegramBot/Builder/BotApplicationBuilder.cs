using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.Runner;
using MinimalTelegramBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBotClientOptions = MinimalTelegramBot.Client.TelegramBotClientOptions;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     Provides a builder for configuring and creating a BotApplication instance.
/// </summary>
public sealed class BotApplicationBuilder : IHostApplicationBuilder
{
    private readonly WebApplicationBuilder _hostBuilder;

    internal BotApplicationBuilder(BotApplicationOptions options)
    {
        _hostBuilder = WebApplication.CreateSlimBuilder(options.WebApplicationOptions ?? new WebApplicationOptions());

        ApplyDefaultConfiguration(_hostBuilder.Configuration, _hostBuilder.Services, options);
        AddDefaultServices(_hostBuilder.Services);
    }

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplicationBuilder.Configuration"/>
    public ConfigurationManager Configuration => _hostBuilder.Configuration;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplicationBuilder.Services"/>
    public IServiceCollection Services => _hostBuilder.Services;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplicationBuilder.Logging"/>
    public ILoggingBuilder Logging => _hostBuilder.Logging;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplicationBuilder.Environment"/>
    public IWebHostEnvironment Environment => _hostBuilder.Environment;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplicationBuilder.Metrics"/>
    public IMetricsBuilder Metrics => _hostBuilder.Metrics;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplicationBuilder.Host"/>
    public ConfigureHostBuilder Host => _hostBuilder.Host;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplicationBuilder.WebHost"/>
    public ConfigureWebHostBuilder WebHost => _hostBuilder.WebHost;

    /// <summary>
    ///     Provides access to the <see cref="WebApplicationBuilder"/> under the <see cref="BotApplicationBuilder"/>.
    /// </summary>
    public WebApplicationBuilder WebApplicationBuilderAccessor => _hostBuilder;

    IDictionary<object, object> IHostApplicationBuilder.Properties => ((IHostApplicationBuilder)_hostBuilder).Properties;
    IConfigurationManager IHostApplicationBuilder.Configuration => _hostBuilder.Configuration;
    IHostEnvironment IHostApplicationBuilder.Environment => _hostBuilder.Environment;

    void IHostApplicationBuilder.ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure)
    {
        ((IHostApplicationBuilder)_hostBuilder).ConfigureContainer(factory, configure);
    }

    /// <summary>
    ///     Builds a new instance of <see cref="BotApplication"/>.
    /// </summary>
    /// <returns>A new instance of <see cref="BotApplication"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the bot token is not configured.</exception>
    public BotApplication Build()
    {
        _hostBuilder.Services.AddHostedService<BotHostedService>();
        var host = _hostBuilder.Build();
        return new BotApplication(host);
    }

    private static void ApplyDefaultConfiguration(ConfigurationManager configuration, IServiceCollection services, BotApplicationOptions options)
    {
        services.Configure<ReceiverOptions>(o =>
        {
            o.DropPendingUpdates = true;
            o.AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery];
        });

        services.PostConfigure((TelegramBotClientOptions o) =>
        {
            o.Token ??= options.Token ??
                        configuration["TelegramBotToken"] ??
                        configuration["BotToken"] ??
                        configuration["Token"];
        });
    }

    private static void AddDefaultServices(IServiceCollection services)
    {
        services.AddSingleton<ITelegramBotClient>(s =>
        {
            var options = s.GetRequiredService<IOptions<TelegramBotClientOptions>>().Value;

            if (options.Token is null)
            {
                throw new InvalidOperationException("Cannot instantiate a bot client without a configured bot token.");
            }

            var ctorOptions = new Telegram.Bot.TelegramBotClientOptions(options.Token, options.BaseUrl, options.UseTestEnvironment)
            {
                RetryCount = options.RetryCount,
                RetryThreshold = options.RetryThreshold
            };

            return new TelegramBotClient(ctorOptions);
        });

        services.AddSingleton<IBotRequestContextAccessor, BotRequestContextAccessor>();
        services.AddSingleton(new BotApplicationContainer());
        services.AddSingleton(new WebApplicationConfiguration());
    }
}
