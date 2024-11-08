namespace MinimalTelegramBot.Pipeline;

/// <summary>
///     Represents a pipe (middleware) to use in <see cref="BotApplication"/> bot request execution pipeline.
/// </summary>
public interface IPipe
{
    /// <summary>
    ///     Invokes the pipe with the given context and the next delegate in the pipeline.
    /// </summary>
    /// <param name="context">The context of the bot request.</param>
    /// <param name="next">The next delegate to be invoked in the pipeline.</param>
    /// <returns>A task that represents an asynchronous operation.</returns>
    Task InvokeAsync(BotRequestContext context, BotRequestDelegate next);
}
