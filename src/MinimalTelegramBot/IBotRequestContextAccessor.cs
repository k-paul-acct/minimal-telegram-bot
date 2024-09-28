namespace MinimalTelegramBot;

/// <summary>
///     Provides access to the current <see cref="BotRequestContext"/>.
/// </summary>
public interface IBotRequestContextAccessor
{
    /// <summary>
    ///     Gets or sets the current <see cref="BotRequestContext"/>.
    /// </summary>
    BotRequestContext? BotRequestContext { get; set; }
}
