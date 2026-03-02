using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.Logging;
using MinimalTelegramBot.Server;
using MinimalTelegramBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Http = Microsoft.AspNetCore.Http;

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
    private readonly WebApplication _host;

    internal BotApplication(WebApplication host)
    {
        _host = host;
        _properties = new Dictionary<string, object?>();
        _pipelineBuilder = new PipelineBuilder(Services, _properties);
        _handlerDispatcher = new RootHandlerDispatcher(Services);

        UseDefaultPipes(_pipelineBuilder, _host.Services);
    }

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplication.Services"/>
    public IServiceProvider Services => _host.Services;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplication.Configuration"/>
    public IConfiguration Configuration => _host.Configuration;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplication.Environment"/>
    public IWebHostEnvironment Environment => _host.Environment;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplication.Lifetime"/>
    public IHostApplicationLifetime Lifetime => _host.Lifetime;

    /// <inheritdoc cref="Microsoft.AspNetCore.Builder.WebApplication.Logger"/>
    public ILogger Logger => _host.Logger;

    /// <summary>
    ///     Provides access to the <see cref="WebApplication"/> under the <see cref="BotApplication"/>.
    /// </summary>
    public WebApplication WebApplicationAccessor => _host;

    ICollection<IHandlerSource> IHandlerDispatcher.HandlerSources => _handlerDispatcher.HandlerSources;
    IDictionary<string, object?> IBotApplicationBuilder.Properties => _properties;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BotApplicationBuilder"/> with preconfigured defaults.
    /// </summary>
    /// <returns>The <see cref="BotApplicationBuilder"/>.</returns>
    public static BotApplicationBuilder CreateBuilder()
    {
        return CreateBuilder(new BotApplicationOptions
        {
            WebApplicationOptions = new WebApplicationOptions()
        });
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BotApplicationBuilder"/> with preconfigured defaults.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The <see cref="BotApplicationBuilder"/>.</returns>
    public static BotApplicationBuilder CreateBuilder(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        return CreateBuilder(new BotApplicationOptions
        {
            WebApplicationOptions = new WebApplicationOptions
            {
                Args = args
            }
        });
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="BotApplicationBuilder"/> with preconfigured defaults.
    /// </summary>
    /// <param name="options">The <see cref="BotApplicationOptions"/> to configure the <see cref="BotApplicationBuilder"/>.</param>
    /// <returns>The <see cref="BotApplicationBuilder"/>.</returns>
    public static BotApplicationBuilder CreateBuilder(BotApplicationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
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
        return _pipelineBuilder.Build();
    }

    /// <inheritdoc/>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        ApplyPipelineConfiguration();

        var pipeline = _pipelineBuilder.Build();
        var updateServer = new UpdateServer(_host.Services, pipeline);
        var isWebhook = _properties.ContainsKey("__WebhookEnabled");

        var dispatchFunc = isWebhook
            ? SetupApplicationForWebhook(updateServer)
            : SetupApplicationForPolling(updateServer);

        var container = _host.Services.GetRequiredService<BotApplicationContainer>();
        container.BotApplicationBuilder = this;
        container.DispatchFunc = dispatchFunc;

        return _host.StartAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _host.StopAsync(cancellationToken);
    }

    void IDisposable.Dispose()
    {
        ((IDisposable)_host).Dispose();
    }

    private Func<CancellationToken, Task> SetupApplicationForPolling(UpdateServer updateServer)
    {
        if (_properties.TryGetValue("__PollingEnabled", out var pollingBuilder))
        {
            var pollingConfiguration = ((PollingBuilder)pollingBuilder!).Build();

            if (pollingConfiguration.Url is not null)
            {
                var webApplicationConfiguration = _host.Services.GetRequiredService<WebApplicationConfiguration>();
                webApplicationConfiguration.BaseUrl = new Uri(pollingConfiguration.Url);
            }

            pollingConfiguration.StaticFilesAction?.Invoke(_host);
        }

        return ct =>
        {
            var botClient = this.Services.GetRequiredService<ITelegramBotClient>();
            var options = this.Services.GetRequiredService<IOptions<ReceiverOptions>>().Value;
            var loggerFactory = this.Services.GetRequiredService<ILoggerFactory>();
            var logger = InfrastructureLog.CreateLogger(loggerFactory);

            botClient.StartReceiving(UpdateHandler, PollingErrorHandler, options, CancellationToken.None);

            return Task.CompletedTask;

            Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
            {
                var invocationContext = updateServer.CreatePollingInvocationContext(update);
                _ = Task.Run(() => updateServer.Serve(invocationContext), cancellationToken);
                return Task.CompletedTask;
            }

            Task PollingErrorHandler(ITelegramBotClient client, Exception ex, CancellationToken cancellationToken)
            {
                InfrastructureLog.PollingError(logger, ex);
                return Task.CompletedTask;
            }
        };
    }

    private Func<CancellationToken, Task> SetupApplicationForWebhook(UpdateServer updateServer)
    {
        var webhookBuilder = (WebhookBuilder)_properties["__WebhookEnabled"]!;
        var webhookConfiguration = webhookBuilder.Build();

        var webApplicationConfiguration = _host.Services.GetRequiredService<WebApplicationConfiguration>();
        webApplicationConfiguration.BaseUrl = new Uri(webhookConfiguration.Options.Url);

        if (webhookConfiguration.DeleteWebhookOnShutdown)
        {
            _properties["__DeleteWebhookOnShutdown"] = new object();
        }

        _host.UseStaticFiles();

        if (!_host.Environment.IsDevelopment())
        {
            _host.UseExceptionHandler(builder => builder.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Text.Plain;
                await context.Response.WriteAsync("Internal Server Error");
            }));
        }

        var routeHandlerBuilder = webhookConfiguration.WebhookResponseEnabled
            ? _host.MapPost(webhookConfiguration.ListenPath, async (Update update) =>
            {
                var invocationContext = updateServer.CreateWebhookInvocationContext(update);
                var httpContentTask = invocationContext.WebhookTelegramBotClient.WaitHttpContent();
                _ = Task.Run(() => updateServer.Serve(invocationContext));
                var httpContent = await httpContentTask;
                return httpContent is null ? Http.Results.StatusCode(StatusCodes.Status200OK) : new JsonHttpContentResult(httpContent);
            })
            : _host.MapPost(webhookConfiguration.ListenPath, async (Update update) =>
            {
                var invocationContext = updateServer.CreatePollingInvocationContext(update);
                await updateServer.Serve(invocationContext);
                return Http.Results.StatusCode(StatusCodes.Status200OK);
            });

        if (webhookConfiguration.Options.SecretToken is not null)
        {
            routeHandlerBuilder.AddEndpointFilter(new TelegramWebhookSecretTokenFilter(webhookConfiguration.Options.SecretToken));
        }

        return ct =>
        {
            if (webhookConfiguration.SkipWebhookSettingOnStartup)
            {
                return Task.CompletedTask;
            }

            var botClient = this.Services.GetRequiredService<ITelegramBotClient>();
            var options = this.Services.GetRequiredService<IOptions<ReceiverOptions>>().Value;

            return botClient.SetWebhook(
                webhookConfiguration.Options.Url,
                webhookConfiguration.Options.Certificate,
                webhookConfiguration.Options.IpAddress,
                webhookConfiguration.Options.MaxConnections,
                options.AllowedUpdates,
                options.DropPendingUpdates,
                webhookConfiguration.Options.SecretToken,
                ct);
        };
    }

    private void ApplyPipelineConfiguration()
    {
        if (_pipelineBuilder.Properties.ContainsKey("__CallbackAutoAnsweringPipeAdded"))
        {
            _pipelineBuilder.UsePipe(new CallbackAutoAnsweringPipe());
        }

        var handlerResolverPipe = HandlerResolverPipeBuilder.Build(_handlerDispatcher.HandlerSources);
        _pipelineBuilder.Use(handlerResolverPipe);
    }

    private static void UseDefaultPipes(PipelineBuilder pipeline, IServiceProvider services)
    {
        pipeline.UsePipe(new UpdateLoggingPipe(services.GetRequiredService<ILoggerFactory>()));
        pipeline.UsePipe(new BotRequestContextAccessorPipe(services.GetRequiredService<IBotRequestContextAccessor>()));
    }

    private sealed class RootHandlerDispatcher : IHandlerDispatcher
    {
        public RootHandlerDispatcher(IServiceProvider services)
        {
            Services = services;
            HandlerSources = new List<IHandlerSource>();
        }

        public IServiceProvider Services { get; }
        public ICollection<IHandlerSource> HandlerSources { get; }
    }

    private sealed class JsonHttpContentResult : Http.IResult
    {
        private readonly HttpContent _httpContent;

        public JsonHttpContentResult(HttpContent httpContent)
        {
            _httpContent = httpContent;
        }

        public Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCodes.Status200OK;
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            httpContext.Response.RegisterForDispose(_httpContent);
            return _httpContent.CopyToAsync(httpContext.Response.Body);
        }
    }

    private sealed class TelegramWebhookSecretTokenFilter : IEndpointFilter
    {
        private const string SecretTokenHeaderName = "X-Telegram-Bot-Api-Secret-Token";

        private readonly string _secretToken;

        public TelegramWebhookSecretTokenFilter(string secretToken)
        {
            _secretToken = secretToken;
        }

        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var header = context.HttpContext.Request.Headers[SecretTokenHeaderName];

            if (header.Count != 1)
            {
                return new ValueTask<object?>(Http.Results.StatusCode(StatusCodes.Status401Unauthorized));
            }

            return header.Equals(_secretToken)
                ? next(context)
                : new ValueTask<object?>(Http.Results.StatusCode(StatusCodes.Status401Unauthorized));
        }
    }
}
