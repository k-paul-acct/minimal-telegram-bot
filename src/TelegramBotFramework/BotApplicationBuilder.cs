using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Telegram.Bot;
using TelegramBotFramework.Commands;
using TelegramBotFramework.Pipeline.Default;
using TelegramBotFramework.Services;

namespace TelegramBotFramework;

public class BotApplicationBuilder
{
    private readonly BotApplicationBuilderOptions _options = new();

    internal BotApplicationBuilder(string[]? args)
    {
        HostBuilder = args is not null ? Host.CreateApplicationBuilder(args) : Host.CreateApplicationBuilder();
        AddDefaultServices();
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

    public BotApplicationBuilder AddCommands()
    {
        _options.CommandsAssembly = Assembly.GetCallingAssembly();
        return this;
    }

    public BotApplication Build()
    {
        _options.Validate();

        if (_options.CommandsAssembly is not null)
        {
            FindAndApplyCommands(_options.CommandsAssembly);
        }

        var client = new TelegramBotClient(_options.Token!);
        HostBuilder.Services.AddSingleton(client);
        var host = HostBuilder.Build();

        return new BotApplication(host, client, new BotApplicationOptions(_options));
    }

    private void FindAndApplyCommands(Assembly assembly)
    {
        var commandTypes = assembly
            .GetTypes()
            .Where(x => x.IsClass && typeof(ICommand).IsAssignableFrom(x));

        foreach (var commandType in commandTypes)
        {
            if (Attribute.GetCustomAttribute(commandType, typeof(CommandAttribute)) is not CommandAttribute attribute)
            {
                throw new Exception("No command specified");
            }

            HostBuilder.Services.AddKeyedScoped(typeof(ICommand), attribute.Command, commandType);
        }
    }

    private void AddDefaultPipeServices()
    {
        HostBuilder.Services.TryAddScoped<ExceptionHandlerPipe>();
        HostBuilder.Services.TryAddScoped<UpdateLoggerPipe>();
    }

    private void AddDefaultServices()
    {
        HostBuilder.Services.AddTransient<BotInitService>();
    }
}