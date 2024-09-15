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
        await app._host.StartAsync();

        app._client.StartReceiving(UpdateHandler, PollingErrorHandler, app._options.ReceiverOptions);

        return app._host;

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
}
