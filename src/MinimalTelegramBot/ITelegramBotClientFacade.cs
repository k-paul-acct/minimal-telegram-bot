using Telegram.Bot;

namespace MinimalTelegramBot;

public interface ITelegramBotClientFacade : ITelegramBotClient
{
    Task<HttpContent?> WaitHttpContent();
    public void FlushHttpContent();
}
