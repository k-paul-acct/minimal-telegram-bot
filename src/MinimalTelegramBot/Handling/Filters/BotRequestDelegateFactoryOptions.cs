namespace MinimalTelegramBot.Handling.Filters;

/// <summary>
///     Options for creating a <see cref="BotRequestDelegate"/>.
/// </summary>
public sealed class BotRequestDelegateFactoryOptions
{
    /// <summary>
    ///     Gets the <see cref="IServiceProvider"/> used to resolve services.
    /// </summary>
    public required IServiceProvider Services { get; init; }

    /// <summary>
    ///     Gets the <see cref="HandlerBuilder"/> used to build request handlers.
    /// </summary>
    public required HandlerBuilder HandlerBuilder { get; init; }
}
