using MinimalTelegramBot.Client;
using MinimalTelegramBot.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Server;

internal sealed class UpdateServer
{
    private readonly ITelegramBotClient _client;
    private readonly InfrastructureLogger _logger;
    private readonly BotRequestDelegate _pipeline;
    private readonly IServiceProvider _services;

    public UpdateServer(IServiceProvider services, BotRequestDelegate pipeline, InfrastructureLogger logger)
    {
        _services = services;
        _pipeline = pipeline;
        _logger = logger;
        _client = _services.GetRequiredService<ITelegramBotClient>();
    }

    public UpdateServerPollingInvocationContext CreatePollingInvocationContext(Update update)
    {
        var scope = _services.CreateAsyncScope();
        var context = new BotRequestContext(scope.ServiceProvider, update, _client);
        return new UpdateServerPollingInvocationContext(scope, context);
    }

    public UpdateServerWebhookInvocationContext CreateWebhookInvocationContext(Update update)
    {
        var scope = _services.CreateAsyncScope();
        var client = new WebhookTelegramBotClient(_client);
        var context = new BotRequestContext(scope.ServiceProvider, update, client);
        return new UpdateServerWebhookInvocationContext(scope, context, client);
    }

    public async Task Serve(UpdateServerInvocationContext invocationContext)
    {
        await using (invocationContext)
        {
            try
            {
                await _pipeline(invocationContext.BotRequestContext);
            }
            catch (Exception ex)
            {
                _logger.ApplicationError(ex);
            }
        }
    }
}
