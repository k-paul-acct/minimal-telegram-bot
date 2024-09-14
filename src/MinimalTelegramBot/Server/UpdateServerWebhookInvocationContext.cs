using MinimalTelegramBot.Client;
using Telegram.Bot;

namespace MinimalTelegramBot.Server;

internal sealed class UpdateServerWebhookInvocationContext : UpdateServerInvocationContext
{
    public UpdateServerWebhookInvocationContext(AsyncServiceScope serviceScope, BotRequestContext botRequestContext, ITelegramBotClient telegramBotClient)
        : base(serviceScope, botRequestContext)
    {
        WebhookTelegramBotClient = new WebhookTelegramBotClient(telegramBotClient);
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
