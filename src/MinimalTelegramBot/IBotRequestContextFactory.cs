using Telegram.Bot.Types;

namespace MinimalTelegramBot;

public interface IBotRequestContextFactory
{
    Task<BotRequestContext> Create(IServiceProvider services, Update update, bool webhookResponseAvailable);
}
