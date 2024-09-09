using MinimalTelegramBot.Pipeline.TypedPipes;
using MinimalTelegramBot.Runner;
using MinimalTelegramBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Builder;

public sealed class BotApplication : IBotApplicationBuilder, IHandlerDispatcher, IHost
{
    private readonly PipelineBuilder _pipelineBuilder;
    private readonly Dictionary<string, object?> _properties;
    private readonly IHandlerDispatcher _handlerDispatcher;

    internal readonly IHost _host;
    internal readonly TelegramBotClient _client;
    internal readonly BotApplicationOptions _options;

    internal BotApplication(IHost host, TelegramBotClient client, BotApplicationOptions options)
    {
        _host = host;
        _client = client;
        _options = options;
        _properties = new Dictionary<string, object?>();
        _pipelineBuilder = new PipelineBuilder(Services, _properties);
        _handlerDispatcher = new RootHandlerDispatcher(Services);

        this.UsePipe<UpdateLoggerPipe>();
    }

    public IServiceProvider Services => _host.Services;
    ICollection<HandlerSource> IHandlerDispatcher.HandlerSources => _handlerDispatcher.HandlerSources;
    IDictionary<string, object?> IBotApplicationBuilder.Properties => _properties;

    public static BotApplicationBuilder CreateBuilder()
    {
        return CreateBuilder(new BotApplicationBuilderOptions());
    }

    public static BotApplicationBuilder CreateBuilder(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        return CreateBuilder(new BotApplicationBuilderOptions
        {
            Args = args,
            HostApplicationBuilderSettings = new HostApplicationBuilderSettings
            {
                Args = args,
            },
        });
    }

    public static BotApplicationBuilder CreateBuilder(HostApplicationBuilderSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return CreateBuilder(new BotApplicationBuilderOptions
        {
            Args = settings.Args,
            HostApplicationBuilderSettings = settings,
        });
    }

    private static BotApplicationBuilder CreateBuilder(BotApplicationBuilderOptions options)
    {
        options.ReceiverOptions.AllowedUpdates ??= [UpdateType.Message, UpdateType.CallbackQuery,];
        return new BotApplicationBuilder(options);
    }

    public IBotApplicationBuilder Use(Func<BotRequestDelegate, BotRequestDelegate> pipe)
    {
        ArgumentNullException.ThrowIfNull(pipe);

        _pipelineBuilder.Use(pipe);
        return this;
    }

    BotRequestDelegate IBotApplicationBuilder.Build()
    {
        var handlerResolver = new HandlerResolver(_handlerDispatcher.HandlerSources);
        var fullPipeline = handlerResolver.BuildFullPipeline();

        this.Use(next => context => fullPipeline(context, next));

        var pipeline = _pipelineBuilder.Build();

        return pipeline;
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return _host.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _host.StopAsync(cancellationToken);
    }

    public void Dispose()
    {
        _host.Dispose();
    }

    public void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    public Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (_pipelineBuilder.Properties.ContainsKey("__CallbackAutoAnsweringAdded"))
        {
            this.UsePipe(new CallbackAutoAnsweringPipe());
        }

        return BotApplicationRunner.RunAsync(this, cancellationToken);
    }
}
