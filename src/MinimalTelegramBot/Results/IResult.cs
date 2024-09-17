namespace MinimalTelegramBot.Results;

/// <summary>
///     Represents the interface for some executable result as an action on an update.
/// </summary>
public interface IResult
{
    /// <summary>
    ///     Executes the result asynchronously.
    /// </summary>
    /// <param name="context">The bot request context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(BotRequestContext context);
}
