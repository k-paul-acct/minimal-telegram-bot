using System.Net.Mime;
using Microsoft.AspNetCore.Http.Json;
using MinimalTelegramBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using Http = Microsoft.AspNetCore.Http;

namespace MinimalTelegramBot.Runner;

internal static class WebhookRunner
{
    public static async Task<IHost> StartWebhook(this BotApplication app, UpdateHandler updateHandler, CancellationToken ct)
    {
        IBotApplicationBuilder builder = app;
        var options = (WebhookOptions)builder.Properties["__WebhookEnabled"]!;

        await app._client.SetWebhookAsync(
            options.Url, options.Certificate, options.IpAddress, options.MaxConnections, app._options.ReceiverOptions?.AllowedUpdates,
            app._options.ReceiverOptions?.DropPendingUpdates ?? false, options.SecretToken, ct);

        var webApp = CreateWebApp(app, options, updateHandler);

        await webApp.StartAsync(ct);

        return webApp;
    }

    private static WebApplication CreateWebApp(BotApplication app, WebhookOptions options, UpdateHandler updateHandler)
    {
        var webAppBuilder = WebApplication.CreateSlimBuilder(app._options.Args ?? []);

        webAppBuilder.Services.Configure<JsonOptions>(o => JsonBotAPI.Configure(o.SerializerOptions));

        var webApp = webAppBuilder.Build();

        webApp.MapPost("", async (Update update) =>
        {
            var invocationContext = await updateHandler.CreateInvocationContext(update, true);
            if (invocationContext is null)
            {
                return Http.Results.StatusCode(StatusCodes.Status500InternalServerError);
            }

            var httpContentTask = invocationContext.BotRequestContext.Client.WaitHttpContent();
            _ = Task.Run(invocationContext.Invoke);
            var httpContent = await httpContentTask;
            return httpContent is null ? Http.Results.StatusCode(StatusCodes.Status200OK) : new JsonHttpContentResult(httpContent);
        }).AddEndpointFilter(new TelegramWebhookSecretTokenFilter(options.SecretToken));

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
            using (_httpContent)
            {
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
                httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                _httpContent.CopyTo(httpContext.Response.Body, null, default);
                httpContext.Response.ContentLength = _httpContent.Headers.ContentLength;
                return Task.CompletedTask;
            }
        }
    }
}
