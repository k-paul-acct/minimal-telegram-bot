namespace MinimalTelegramBot;

public interface ITelegramBotClientFactory
{
    ITelegramBotClientFacade Create(bool webhookResponseAvailable);
}
