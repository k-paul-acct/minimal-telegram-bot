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
    internal readonly IDictionary<string, object?> _properties;

    public UpdateServer(IServiceProvider services, BotRequestDelegate pipeline, IDictionary<string, object?> properties, InfrastructureLogger logger)
    {
        _services = services;
        _pipeline = pipeline;
        _properties = properties;
        _logger = logger;
        _client = _services.GetRequiredService<ITelegramBotClient>();
    }

    public UpdateServerPollingInvocationContext CreatePollingInvocationContext(Update update)
    {
        var scope = _services.CreateAsyncScope();
        var context = new BotRequestContext(scope.ServiceProvider, update, _client, _properties);
        return new UpdateServerPollingInvocationContext(scope, context);
    }

    public UpdateServerWebhookInvocationContext CreateWebhookInvocationContext(Update update)
    {
        var scope = _services.CreateAsyncScope();
        var client = new WebhookTelegramBotClient(_client);
        var context = new BotRequestContext(scope.ServiceProvider, update, client, _properties);
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
