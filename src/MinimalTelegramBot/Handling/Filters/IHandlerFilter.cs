namespace MinimalTelegramBot.Handling.Filters;

/// <summary>
///     Represents a filter for a <see cref="Handler"/>.
/// </summary>
public interface IHandlerFilter
{
    /// <summary>
    ///     Executes the filter.
    /// </summary>
    /// <param name="context">The context for the filter.</param>
    /// <param name="next">The delegate to invoke the next filter in the pipeline.</param>
    /// <returns>A task that represents an asynchronous operation, containing the result of the filter pipeline execution.</returns>
    ValueTask<IResult> InvokeAsync(BotRequestFilterContext context, BotRequestFilterDelegate next);
}
