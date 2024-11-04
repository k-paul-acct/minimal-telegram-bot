namespace MinimalTelegramBot.Handling.Filters;

/// <summary>
/// </summary>
public sealed class BotRequestDelegateFactoryOptions
{
    /// <summary>
    /// </summary>
    public required IServiceProvider Services { get; init; }

    /// <summary>
    /// </summary>
    public required HandlerBuilder HandlerBuilder { get; init; }
}
