using Microsoft.AspNetCore.Http.Json;
using MinimalTelegramBot.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using HttpResults = Microsoft.AspNetCore.Http.Results;

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
            await updateHandler.Handle(update);
            return HttpResults.StatusCode(StatusCodes.Status200OK);
        }).AddEndpointFilter(new TelegramWebhookSecretTokenFilter(options.SecretToken));

        return webApp;
    }
}
