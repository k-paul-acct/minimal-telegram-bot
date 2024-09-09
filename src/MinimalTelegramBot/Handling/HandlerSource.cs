namespace MinimalTelegramBot.Handling;

public abstract class HandlerSource
{
    public abstract IReadOnlyCollection<Handler> Handlers { get; }
}
