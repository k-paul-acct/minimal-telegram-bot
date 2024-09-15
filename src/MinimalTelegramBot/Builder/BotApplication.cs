using Microsoft.Extensions.Hosting;
using MinimalTelegramBot.Pipeline.TypedPipes;
using MinimalTelegramBot.Runner;
using MinimalTelegramBot.Settings;
using Telegram.Bot;

namespace MinimalTelegramBot.Builder;

public sealed class BotApplication : IBotApplicationBuilder, IHandlerDispatcher, IHost
{
    private readonly PipelineBuilder _pipelineBuilder;
    private readonly Dictionary<string, object?> _properties;
    private readonly IHandlerDispatcher _handlerDispatcher;

    internal readonly IHost _host;
    internal readonly ITelegramBotClient _client;
    internal readonly BotApplicationOptions _options;

    internal BotApplication(IHost host, ITelegramBotClient client, BotApplicationOptions options)
    {
        _host = host;
        _client = client;
        _options = options;
        _properties = new Dictionary<string, object?>();
        _pipelineBuilder = new PipelineBuilder(Services, _properties);
        _handlerDispatcher = new RootHandlerDispatcher(Services);

        UsePipesBeforeHandlerResolver();
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

    private static BotApplicationBuilder CreateBuilder(BotApplicationBuilderOptions options)
    {
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
        var handlerResolverPipe = HandlerResolverPipeBuilder.Build(_handlerDispatcher.HandlerSources);

        this.Use(handlerResolverPipe);

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
        UsePipesBeforeRun();
        return BotApplicationRunner.RunAsync(this, cancellationToken);
    }

    private void UsePipesBeforeHandlerResolver()
    {
        this.UsePipe<UpdateLoggingPipe>();
        this.UsePipe<BotRequestContextAccessorPipe>();
    }

    private void UsePipesBeforeRun()
    {
        if (_pipelineBuilder.Properties.ContainsKey("__CallbackAutoAnsweringPipeAdded"))
        {
            this.UsePipe(new CallbackAutoAnsweringPipe());
        }
    }
}
