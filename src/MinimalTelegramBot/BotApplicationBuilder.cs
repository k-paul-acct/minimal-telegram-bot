using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Pipeline;
using MinimalTelegramBot.Services;
using MinimalTelegramBot.Settings;
using Telegram.Bot;

namespace MinimalTelegramBot;

public class BotApplicationBuilder
{
    private readonly BotApplicationBuilderOptions _options;

    internal BotApplicationBuilder(string[]? args)
    {
        HostBuilder = Host.CreateApplicationBuilder(args);
        _options = new BotApplicationBuilderOptions { Args = args, };
        AddDefaultPipeServices();
    }

    internal BotApplicationBuilder(BotApplicationBuilderOptions options)
    {
        HostBuilder = Host.CreateApplicationBuilder(options.Args);
        _options = options;
        AddDefaultPipeServices();
    }

    public HostApplicationBuilder HostBuilder { get; }

    public BotApplicationBuilder SetToken(string token)
    {
        _options.Token = token ?? throw new Exception("Bot token not found");
        return this;
    }

    public BotApplicationBuilder SetTokenFromConfiguration(string tokenKey)
    {
        _options.Token = HostBuilder.Configuration[tokenKey] ?? throw new Exception("Bot token not found");
        return this;
    }

    public BotApplicationBuilder ConfigureTelegramBotClientOptions(Action<TelegramBotClientOptions> configure)
    {
        _options.TelegramBotClientOptionsConfigure = configure;
        return this;
    }

    public BotApplication Build()
    {
        _options.Validate();

        var telegramBotClientOptions = new TelegramBotClientOptions(_options.Token!);
        _options.TelegramBotClientOptionsConfigure?.Invoke(telegramBotClientOptions);

        var client = new TelegramBotClient(telegramBotClientOptions);
        var handlerBuilder = new HandlerBuilder();

        HostBuilder.Services.AddSingleton<ITelegramBotClient>(client);
        HostBuilder.Services.AddSingleton<BotInitService>();
        HostBuilder.Services.AddSingleton<IHandlerBuilder>(handlerBuilder);
        HostBuilder.Services.TryAddSingleton<IBotRequestContextAccessor, BotRequestContextAccessor>();

        var host = HostBuilder.Build();

        return new BotApplication(host, client, new BotApplicationOptions(_options), handlerBuilder);
    }

    private void AddDefaultPipeServices()
    {
        HostBuilder.Services.AddSingleton<UpdateLoggerPipe>();
        HostBuilder.Services.AddSingleton<HandlerResolverPipe>();
        HostBuilder.Services.AddSingleton<ExceptionHandlerPipe>();
        HostBuilder.Services.AddSingleton<CallbackAutoAnsweringPipe>();
    }
}