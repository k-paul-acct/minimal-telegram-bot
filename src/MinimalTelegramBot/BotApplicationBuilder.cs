using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Pipeline;
using MinimalTelegramBot.Services;
using MinimalTelegramBot.Settings;
using Telegram.Bot;

namespace MinimalTelegramBot;

public class BotApplicationBuilder
{
    private readonly BotApplicationBuilderOptions _options = new();

    internal BotApplicationBuilder(string[]? args)
    {
        HostBuilder = args is not null ? Host.CreateApplicationBuilder(args) : Host.CreateApplicationBuilder();
        AddDefaultPipeServices();
    }

    internal BotApplicationBuilder(BotApplicationBuilderOptions options) : this(options.Args)
    {
        _options = options;
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

    public BotApplication Build()
    {
        _options.Validate();

        var client = new TelegramBotClient(_options.Token!);
        var handlerBuilder = new HandlerBuilder();

        HostBuilder.Services.AddSingleton(client);
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