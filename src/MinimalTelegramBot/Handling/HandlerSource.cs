namespace MinimalTelegramBot.Handling;

public interface HandlerSource
{
    IReadOnlyList<Handler> GetHandlers(IReadOnlyList<Action<HandlerBuilder>> conventions);
}
