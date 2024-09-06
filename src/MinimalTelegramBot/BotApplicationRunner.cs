using Microsoft.AspNetCore.Http.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot;

internal static class BotApplicationRunner
{
    public static async Task RunAsync(BotApplication app, CancellationToken cancellationToken)
    {
        IBotApplicationBuilder appBuilder = app;

        appBuilder.Build();

        var isWebhook = appBuilder.Properties.ContainsKey("__WebhookEnabled");

        appBuilder.Properties.TryGetValue("__DeleteWebhook", out var deleteWebhook);

        if (deleteWebhook is true)
        {
            await DeleteWebhook(app, cancellationToken);
        }

        var host = isWebhook ? await SetupWebhooks(app, cancellationToken) : app.Host;

        if (!isWebhook)
        {
            app.StartPolling();
        }

        await app.InitBot(isWebhook, cancellationToken);

        if (isWebhook)
        {
            await ((WebApplication)host).RunAsync(cancellationToken);
        }
        else
        {
            await host.RunAsync(cancellationToken);
        }
    }

    private static async Task DeleteWebhook(BotApplication app, CancellationToken cancellationToken)
    {
        await app._client.DeleteWebhookAsync(app._options.ReceiverOptions?.DropPendingUpdates ?? false, cancellationToken);
    }

    private static async Task<WebApplication> SetupWebhooks(BotApplication app, CancellationToken cancellationToken)
    {
        var options = app._options.WebhookOptions ??
                      throw new Exception("No webhook options was specified to use webhook");

        await app._client.SetWebhookAsync(options.Url, options.Certificate, options.IpAddress, options.MaxConnections,
            app._options.ReceiverOptions?.AllowedUpdates, app._options.ReceiverOptions?.DropPendingUpdates ?? false,
            options.SecretToken, cancellationToken);

        var webAppBuilder = WebApplication.CreateSlimBuilder(app._options.Args ?? []);

        webAppBuilder.Services.Configure<JsonOptions>(o => JsonBotAPI.Configure(o.SerializerOptions));

        webAppBuilder.Services.AddHttpClient("tgwebhook")
            .RemoveAllLoggers()
            .AddTypedClient(httpClient => new TelegramBotClient(app._options.Token, httpClient));

        var webApp = webAppBuilder.Build();

        webApp.MapPost("/", async (Update update, TelegramBotClient bot) =>
        {
            await app.HandleUpdateInBackground(bot, update);
            return Microsoft.AspNetCore.Http.Results.StatusCode(StatusCodes.Status200OK);
        }).AddEndpointFilter(new TelegramWebHookSecretTokenFilter(options.SecretToken));

        return webApp;
    }
}
