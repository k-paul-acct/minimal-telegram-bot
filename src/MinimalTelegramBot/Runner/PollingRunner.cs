using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using MinimalTelegramBot.Logging;
using MinimalTelegramBot.Server;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Runner;

internal static class PollingRunner
{
    public static async Task<IHost> StartPolling(this BotApplication app, UpdateServer updateServer, InfrastructureLogger logger)
    {
        var host = app._host;

        if (((IBotApplicationBuilder)app).Properties.TryGetValue("__PollingEnabled", out var pollingBuilder))
        {
            var pollingConfiguration = ((PollingBuilder)pollingBuilder!).Build();

            if (pollingConfiguration.Url is not null)
            {
                updateServer._properties["__WebServerUrl"] = new Uri(pollingConfiguration.Url);
            }

            if (pollingConfiguration.StaticFilesAction is not null)
            {
                host = CreateWebApplication(app._options.Args, pollingConfiguration);
            }
        }

        await host.StartAsync();

        app._client.StartReceiving(UpdateHandler, PollingErrorHandler, app._options.ReceiverOptions);

        return host;

        Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            var invocationContext = updateServer.CreatePollingInvocationContext(update);
            _ = Task.Run(() => updateServer.Serve(invocationContext), cancellationToken);
            return Task.CompletedTask;
        }

        Task PollingErrorHandler(ITelegramBotClient client, Exception ex, CancellationToken cancellationToken)
        {
            logger.PollingError(ex);
            return Task.CompletedTask;
        }
    }

    private static WebApplication CreateWebApplication(string[] args, PollingConfiguration configuration)
    {
        var webAppBuilder = WebApplication.CreateSlimBuilder(args);
        var webApp = webAppBuilder.Build();
        configuration.StaticFilesAction?.Invoke(webApp);
        return webApp;
    }
}
