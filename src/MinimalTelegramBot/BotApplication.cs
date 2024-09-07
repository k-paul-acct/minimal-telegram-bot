using MinimalTelegramBot.Runner;
using MinimalTelegramBot.Settings;
using Telegram.Bot;

namespace MinimalTelegramBot;

public sealed class BotApplication : IBotApplicationBuilder, IHandlerBuilder, IHost
{
    private readonly IHandlerBuilder _handlerBuilder;
    private readonly PipelineBuilder _pipelineBuilder;
    private readonly Dictionary<string, object?> _properties;

    internal readonly IHost _host;
    internal readonly TelegramBotClient _client;
    internal readonly BotApplicationOptions _options;

    internal BotApplication(IHost host, TelegramBotClient client, BotApplicationOptions options, IHandlerBuilder handlerBuilder)
    {
        _host = host;
        _client = client;
        _options = options;
        _handlerBuilder = handlerBuilder;
        _properties = new Dictionary<string, object?>();
        _pipelineBuilder = new PipelineBuilder(Services, _properties);

        UseDefaultOuterPipes();
    }

    public IServiceProvider Services => _host.Services;
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
        return new BotApplicationBuilder(options);
    }

    public IBotApplicationBuilder Use(Func<Func<BotRequestContext, Task>, Func<BotRequestContext, Task>> pipe)
    {
        ArgumentNullException.ThrowIfNull(pipe);

        _pipelineBuilder.Use(pipe);
        return this;
    }

    Func<BotRequestContext, Task> IBotApplicationBuilder.Build()
    {
        return _pipelineBuilder.Build();
    }

    public Handler Handle(Delegate handlerDelegate)
    {
        ArgumentNullException.ThrowIfNull(handlerDelegate);

        return _handlerBuilder.Handle(handlerDelegate);
    }

    public Handler Handle(Func<BotRequestContext, Task> func)
    {
        ArgumentNullException.ThrowIfNull(func);

        return _handlerBuilder.Handle(func);
    }

    ValueTask<Handler?> IHandlerBuilder.TryResolveHandler(BotRequestFilterContext ctx)
    {
        return _handlerBuilder.TryResolveHandler(ctx);
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
            this.UsePipe<CallbackAutoAnsweringPipe>();
        }

        this.UsePipe<HandlerResolverPipe>();
        return BotApplicationRunner.RunAsync(this, cancellationToken);
    }

    private void UseDefaultOuterPipes()
    {
        this.UsePipe<ExceptionHandlerPipe>();
        this.UsePipe<UpdateLoggerPipe>();
    }
}
