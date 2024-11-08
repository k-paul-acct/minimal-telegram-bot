namespace MinimalTelegramBot.Handling;

internal sealed class RootHandlerDispatcher : IHandlerDispatcher
{
    public RootHandlerDispatcher(IServiceProvider services)
    {
        Services = services;
        HandlerSources = new List<IHandlerSource>();
    }

    public IServiceProvider Services { get; }
    public ICollection<IHandlerSource> HandlerSources { get; }
}
