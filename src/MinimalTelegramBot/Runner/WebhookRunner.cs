using System.Net.Mime;
using Microsoft.AspNetCore.Http.Json;
using MinimalTelegramBot.Server;
using MinimalTelegramBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using Http = Microsoft.AspNetCore.Http;

namespace MinimalTelegramBot.Runner;

internal static class WebhookRunner
{
    public static async Task<IHost> StartWebhook(this BotApplication app, UpdateServer updateServer)
    {
        var options = (WebhookOptions)((IBotApplicationBuilder)app).Properties["__WebhookEnabled"]!;
        var webApp = CreateWebApp(app._options.Args, options, updateServer);

        updateServer._properties["__WebhookUrl"] = new Uri(options.Url);

        await webApp.StartAsync();

        await app._client.SetWebhookAsync(options.Url, options.Certificate, options.IpAddress, options.MaxConnections,
            app._options.ReceiverOptions.AllowedUpdates, app._options.ReceiverOptions.DropPendingUpdates, options.SecretToken);

        return webApp;
    }

    private static WebApplication CreateWebApp(string[] args, WebhookOptions options, UpdateServer updateServer)
    {
        var webAppBuilder = WebApplication.CreateSlimBuilder(args);

        webAppBuilder.Services.Configure<JsonOptions>(o => JsonBotAPI.Configure(o.SerializerOptions));

        var webApp = webAppBuilder.Build();

        webApp.UseStaticFiles();

        if (!webApp.Environment.IsDevelopment())
        {
            webApp.UseExceptionHandler(builder => builder.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Text.Plain;
                await context.Response.WriteAsync("Internal Server Error");
            }));
        }

        var routeHandlerBuilder = webApp.MapPost(options.ListenPath, async (Update update) =>
        {
            var invocationContext = updateServer.CreateWebhookInvocationContext(update);
            var httpContentTask = invocationContext.WebhookTelegramBotClient.WaitHttpContent();
            _ = Task.Run(() => updateServer.Serve(invocationContext));
            var httpContent = await httpContentTask;
            return httpContent is null ? Http.Results.StatusCode(StatusCodes.Status200OK) : new JsonHttpContentResult(httpContent);
        });

        if (options.SecretToken is not null)
        {
            routeHandlerBuilder.AddEndpointFilter(new TelegramWebhookSecretTokenFilter(options.SecretToken));
        }

        return webApp;
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
