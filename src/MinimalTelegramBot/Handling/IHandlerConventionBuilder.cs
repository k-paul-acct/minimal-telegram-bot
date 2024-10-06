namespace MinimalTelegramBot.Handling;

/// <summary>
///     Defines a builder for handler conventions such as filters.
/// </summary>
public interface IHandlerConventionBuilder
{
    /// <summary>
    ///     Adds a convention to the handler builder.
    /// </summary>
    /// <param name="convention">The convention to add to the handler builder.</param>
    void Add(Action<HandlerBuilder> convention);
}
