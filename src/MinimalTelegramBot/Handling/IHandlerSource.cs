namespace MinimalTelegramBot.Handling;

/// <summary>
///     A source of handlers.
/// </summary>
public interface IHandlerSource
{
    /// <summary>
    ///     Gets the handlers.
    /// </summary>
    /// <param name="conventions">A list of conventions to apply to the handlers from this source.</param>
    /// <returns>Handlers with the given conventions applied.</returns>
    IReadOnlyList<Handler> GetHandlers(IReadOnlyList<Action<HandlerBuilder>> conventions);
}
