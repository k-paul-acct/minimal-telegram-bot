using MinimalTelegramBot.Client;

namespace MinimalTelegramBot.Server;

internal sealed class UpdateServerWebhookInvocationContext : UpdateServerInvocationContext
{
    public UpdateServerWebhookInvocationContext(AsyncServiceScope serviceScope, BotRequestContext botRequestContext, WebhookTelegramBotClient client)
        : base(serviceScope, botRequestContext)
    {
        WebhookTelegramBotClient = client;
    }

    public WebhookTelegramBotClient WebhookTelegramBotClient { get; }

    public override void Dispose()
    {
        WebhookTelegramBotClient.AbortWaiting();
        base.Dispose();
    }

    public override ValueTask DisposeAsync()
    {
        WebhookTelegramBotClient.AbortWaiting();
        return base.DisposeAsync();
    }
}
