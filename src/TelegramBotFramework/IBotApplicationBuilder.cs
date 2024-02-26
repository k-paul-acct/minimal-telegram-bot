using TelegramBotFramework.Pipeline;

namespace TelegramBotFramework;

public interface IBotApplicationBuilder
{
    IBotApplicationBuilder Use(Func<BotRequestDelegate, BotRequestDelegate> pipe);
    BotRequestDelegate Build();
}