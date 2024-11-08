using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinimalTelegramBot.Runner;
using MinimalTelegramBot.Settings;
using Telegram.Bot;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     Represents the main application for the Telegram Bot, responsible for building pipeline (middleware), handling, filtering,
///     and running the bot.
/// </summary>
public sealed class BotApplication : IBotApplicationBuilder, IHandlerDispatcher, IHost
{
    private readonly PipelineBuilder _pipelineBuilder;
    private readonly Dictionary<string, object?> _properties;
    private readonly RootHandlerDispatcher _handlerDispatcher;

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

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.IHost.Services"/>
    public IServiceProvider Services => _host.Services;

    ICollection<IHandlerSource> IHandlerDispatcher.HandlerSources => _handlerDispatcher.HandlerSources;
    IDictionary<string, object?> IBotApplicationBuilder.Properties => _properties;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BotApplicationBuilder"/> with preconfigured defaults.
    /// </summary>
    /// <returns>The <see cref="BotApplicationBuilder"/>.</returns>
    public static BotApplicationBuilder CreateBuilder()
    {
        return CreateBuilder(new BotApplicationBuilderOptions());
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BotApplicationBuilder"/> with preconfigured defaults.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The <see cref="BotApplicationBuilder"/>.</returns>
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

    /// <inheritdoc cref="MinimalTelegramBot.Pipeline.PipelineBuilder.Use"/>
    public IBotApplicationBuilder Use(Func<BotRequestDelegate, BotRequestDelegate> pipe)
    {
        ArgumentNullException.ThrowIfNull(pipe);

        _pipelineBuilder.Use(pipe);
        return this;
    }

    BotRequestDelegate IBotApplicationBuilder.Build()
    {
        var handlerResolverPipe = HandlerResolverPipeBuilder.Build(_handlerDispatcher.HandlerSources);

        Use(handlerResolverPipe);

        var pipeline = _pipelineBuilder.Build();

        return pipeline;
    }

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.IHost.StartAsync"/>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return _host.StartAsync(cancellationToken);
    }

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.IHost.StopAsync"/>
    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _host.StopAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _host.Dispose();
    }

    /// <summary>
    ///     Synchronously runs the bot application.
    /// </summary>
    public void Run()
    {
        RunAsync().GetAwaiter().GetResult();
    }

    /// <summary>
    ///     Asynchronously runs the bot application.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents an asynchronous operation.</returns>
    public Task RunAsync(CancellationToken cancellationToken = default)
    {
        UsePipesBeforeRun();
        return BotApplicationRunner.RunAsync(this, cancellationToken);
    }

    private void UsePipesBeforeHandlerResolver()
    {
        this.UsePipe(new UpdateLoggingPipe(Services.GetRequiredService<ILogger<UpdateLoggingPipe>>()));
        this.UsePipe(new BotRequestContextAccessorPipe(Services.GetRequiredService<IBotRequestContextAccessor>()));
    }

    private void UsePipesBeforeRun()
    {
        if (_pipelineBuilder.Properties.ContainsKey("__CallbackAutoAnsweringPipeAdded"))
        {
            this.UsePipe(new CallbackAutoAnsweringPipe());
        }
    }
}
