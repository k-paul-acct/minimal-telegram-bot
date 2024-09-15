namespace MinimalTelegramBot.Handling;

public interface IHandlerConventionBuilder
{
    void Add(Action<HandlerBuilder> convention);
}
