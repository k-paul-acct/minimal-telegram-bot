namespace MinimalTelegramBot.Handling;

/// <summary>
///     Represents an entity that can be used as a dispatcher of handlers.
/// </summary>
public interface IHandlerDispatcher
{
    /// <summary>
    ///     Gets the service provider.
    /// </summary>
    IServiceProvider Services { get; }

    /// <summary>
    ///    Gets the handler sources.
    /// </summary>
    ICollection<IHandlerSource> HandlerSources { get; }
}
