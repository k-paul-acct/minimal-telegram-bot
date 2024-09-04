using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.Metrics;
using MinimalTelegramBot.Services;
using MinimalTelegramBot.Settings;
using Telegram.Bot;

namespace MinimalTelegramBot.Builder;

public class BotApplicationBuilder
{
    private readonly HostApplicationBuilder _hostBuilder;

    internal readonly BotApplicationBuilderOptions _options;

    internal BotApplicationBuilder(BotApplicationBuilderOptions options)
    {
        _hostBuilder = Host.CreateApplicationBuilder(options.HostApplicationBuilderSettings);
        _options = options;
        AddDefaultPipeServices();
    }

    public ConfigurationManager Configuration => _hostBuilder.Configuration;
    public IServiceCollection Services => _hostBuilder.Services;
    public ILoggingBuilder Logging => _hostBuilder.Logging;
    public IHostEnvironment Environment => _hostBuilder.Environment;
    public IMetricsBuilder Metrics => _hostBuilder.Metrics;

    public BotApplication Build()
    {
        _options.Validate();

        var telegramBotClientOptions = new TelegramBotClientOptions(_options.Token!);
        _options.TelegramBotClientOptionsConfigure?.Invoke(telegramBotClientOptions);

        var client = new TelegramBotClient(telegramBotClientOptions);
        var handlerBuilder = new HandlerBuilder();

        _hostBuilder.Services.AddSingleton<ITelegramBotClient>(client);
        _hostBuilder.Services.AddSingleton<BotInitService>();
        _hostBuilder.Services.AddSingleton<IHandlerBuilder>(handlerBuilder);
        _hostBuilder.Services.TryAddSingleton<IBotRequestContextAccessor, BotRequestContextAccessor>();

        var host = _hostBuilder.Build();

        return new BotApplication(host, client, new BotApplicationOptions(_options), handlerBuilder);
    }

    private void AddDefaultPipeServices()
    {
        _hostBuilder.Services.AddSingleton<UpdateLoggerPipe>();
        _hostBuilder.Services.AddSingleton<HandlerResolverPipe>();
        _hostBuilder.Services.AddSingleton<ExceptionHandlerPipe>();
        _hostBuilder.Services.AddSingleton<CallbackAutoAnsweringPipe>();
    }
}