namespace TelegramBotFramework.Handling;

public interface IHandlerFilter
{
    Handler Filter(FilterDelegate filterDelegate);
}