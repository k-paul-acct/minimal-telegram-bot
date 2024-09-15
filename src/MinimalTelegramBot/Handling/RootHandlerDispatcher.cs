namespace MinimalTelegramBot.Handling;

internal sealed class RootHandlerDispatcher : IHandlerDispatcher
{
    public RootHandlerDispatcher(IServiceProvider services)
    {
        Services = services;
        HandlerSources = new List<HandlerSource>();
    }

    public IServiceProvider Services { get; }
    public ICollection<HandlerSource> HandlerSources { get; }
}
