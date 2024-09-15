using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinimalTelegramBot.Server;
using Telegram.Bot;
using Telegram.Bot.Types;
using Http = Microsoft.AspNetCore.Http;

namespace MinimalTelegramBot.Runner;

internal static class WebhookRunner
{
    public static async Task<IHost> StartWebhook(this BotApplication app, UpdateServer updateServer)
    {
        var webhookBuilder = (WebhookBuilder)((IBotApplicationBuilder)app).Properties["__WebhookEnabled"]!;
        var webhookConfiguration = webhookBuilder.Build();

        updateServer._properties["__WebhookUrl"] = new Uri(webhookConfiguration.Options.Url);

        if (webhookConfiguration.DeleteWebhookOnShutdown)
        {
            updateServer._properties["__DeleteWebhookOnShutdown"] = new object();
        }

        var webApp = CreateWebApp(app._options.Args, webhookConfiguration, updateServer);

        await webApp.StartAsync();

        await app._client.SetWebhookAsync(
            webhookConfiguration.Options.Url,
            webhookConfiguration.Options.Certificate,
            webhookConfiguration.Options.IpAddress,
            webhookConfiguration.Options.MaxConnections,
            app._options.ReceiverOptions.AllowedUpdates,
            app._options.ReceiverOptions.DropPendingUpdates,
            webhookConfiguration.Options.SecretToken);

        return webApp;
    }

    private static WebApplication CreateWebApp(string[] args, WebhookConfiguration configuration, UpdateServer updateServer)
    {
        WebApplication webApp;

        if (configuration.WebApplication is not null)
        {
            webApp = configuration.WebApplication;
        }
        else
        {
            var webAppBuilder = WebApplication.CreateSlimBuilder(args);
            webAppBuilder.Services.Configure<JsonOptions>(o => JsonBotAPI.Configure(o.SerializerOptions));
            webApp = webAppBuilder.Build();
            webApp.UseStaticFiles();
        }

        if (!webApp.Environment.IsDevelopment())
        {
            webApp.UseExceptionHandler(builder => builder.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Text.Plain;
                await context.Response.WriteAsync("Internal Server Error");
            }));
        }

        if (configuration.WebhookResponseEnabled)
        {
            AddUpdateRouteWithWebhookResponse();
        }
        else
        {
            AddUpdateRoute();
        }

        return webApp;

        void AddUpdateRoute()
        {
            var routeHandlerBuilder = webApp.MapPost(configuration.ListenPath, async (Update update) =>
            {
                var invocationContext = updateServer.CreatePollingInvocationContext(update);
                await updateServer.Serve(invocationContext);
                return Http.Results.StatusCode(StatusCodes.Status200OK);
            });

            if (configuration.Options.SecretToken is not null)
            {
                routeHandlerBuilder.AddEndpointFilter(new TelegramWebhookSecretTokenFilter(configuration.Options.SecretToken));
            }
        }

        void AddUpdateRouteWithWebhookResponse()
        {
            var routeHandlerBuilder = webApp.MapPost(configuration.ListenPath, async (Update update) =>
            {
                var invocationContext = updateServer.CreateWebhookInvocationContext(update);
                var httpContentTask = invocationContext.WebhookTelegramBotClient.WaitHttpContent();
                _ = Task.Run(() => updateServer.Serve(invocationContext));
                var httpContent = await httpContentTask;
                return httpContent is null ? Http.Results.StatusCode(StatusCodes.Status200OK) : new JsonHttpContentResult(httpContent);
            });

            if (configuration.Options.SecretToken is not null)
            {
                routeHandlerBuilder.AddEndpointFilter(new TelegramWebhookSecretTokenFilter(configuration.Options.SecretToken));
            }
        }
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
                : new ValueTask<object?>(Http.Results.StatusCode(StatusCodes.Status403Forbidden));
        }
    }
}
