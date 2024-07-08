namespace TelegramBotFramework;

public interface IBotRequestContextAccessor
{
    BotRequestContext? BotRequestContext { get; set; }
}