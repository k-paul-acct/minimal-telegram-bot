namespace MinimalTelegramBot.Handling;

public interface IHandlerDispatcher
{
    IServiceProvider Services { get; }
    ICollection<HandlerSource> HandlerSources { get; }
}
