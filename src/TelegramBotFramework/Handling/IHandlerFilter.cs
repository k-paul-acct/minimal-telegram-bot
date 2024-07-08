namespace TelegramBotFramework.Handling;

public interface IHandlerFilter
{
    Handler Filter(Func<BotRequestContext, bool> filterDelegate);
}