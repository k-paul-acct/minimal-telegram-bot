using Microsoft.AspNetCore.Http.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using JsonSerializerOptionsProvider = Telegram.Bot.Serialization.JsonSerializerOptionsProvider;

namespace MinimalTelegramBot;

internal static class BotApplicationRunner
{
    public static void Run(BotApplication app)
    {
        ((IBotApplicationBuilder)app).Build();
        var isWebhook = app.Properties.ContainsKey("WebhooksEnabled");

        if (isWebhook)
        {
            var webApp = SetupWebhooks(app).GetAwaiter().GetResult();
            var thread = new Thread(webApp.Run)
            {
                IsBackground = true,
            };
            
            thread.Start();
        }
        else
        {
            app.StartPolling();
        }

        app.InitBot(isWebhook);

        app.Host.Run();
    }

    private static async Task<WebApplication> SetupWebhooks(BotApplication app)
    {
        var options = app.Options.WebhookOptions ??
                      throw new Exception("No webhook options was specified to use webhook");

        // TODO: Test.
        await Task.Delay(1);
        /*await app.Client.SetWebhookAsync(options.Url, options.Certificate, options.IpAddress, options.MaxConnections,
            app.Options.ReceiverOptions?.AllowedUpdates, options.DropPendingUpdates, options.SecretToken);*/

        var webAppBuilder = WebApplication.CreateSlimBuilder(app.Options.Args ?? []);

        webAppBuilder.Services.Configure<JsonOptions>(o =>
        {
            JsonSerializerOptionsProvider.Configure(o.SerializerOptions);
        });
        
        webAppBuilder.Services.AddHttpClient("tgwebhook")
            .RemoveAllLoggers()
            .AddTypedClient(httpClient => new TelegramBotClient(app.Options.Token, httpClient));

        var webApp = webAppBuilder.Build();
        
        webApp.MapPost("/", async (Update update, TelegramBotClient bot) =>
        {
            await app.HandleUpdateInBackground(bot, update);
            return Microsoft.AspNetCore.Http.Results.Ok();
        });

        return webApp;
    }
}