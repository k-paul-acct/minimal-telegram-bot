namespace MinimalTelegramBot.Handling.Filters;

/// <summary>
///     Represents the context for a bot request filter pipeline, providing access to the <see cref="BotRequestContext"/>
///     and additional arguments.
/// </summary>
public sealed class BotRequestFilterContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BotRequestFilterContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="BotRequestContext"/> to create filter context.</param>
    public BotRequestFilterContext(BotRequestContext context)
    {
        BotRequestContext = context;
    }

    /// <summary>
    ///     Gets the arguments for the filter pipeline.
    /// </summary>
    public List<object?> Arguments { get; } = [];

    /// <summary>
    ///     Gets the <see cref="BotRequestContext"/> for the current request.
    /// </summary>
    public BotRequestContext BotRequestContext { get; }

    /// <summary>
    ///     Gets the <see cref="IServiceProvider"/> for the current request.
    /// </summary>
    public IServiceProvider Services => BotRequestContext.Services;
}
