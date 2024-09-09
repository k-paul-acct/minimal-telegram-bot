using Telegram.Bot;

namespace MinimalTelegramBot;

internal sealed class TelegramBotClientFactory : ITelegramBotClientFactory
{
    private readonly ITelegramBotClient _client;

    public TelegramBotClientFactory(ITelegramBotClient client)
    {
        _client = client;
    }

    public ITelegramBotClientFacade Create(bool webhookResponseAvailable)
    {
        return new TelegramBotClientFacade(_client, webhookResponseAvailable);
    }
}
