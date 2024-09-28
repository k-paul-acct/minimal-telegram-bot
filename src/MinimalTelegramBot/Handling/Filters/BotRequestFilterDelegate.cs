namespace MinimalTelegramBot.Handling.Filters;

/// <summary>
///     Represents a method that can be used to filter a bot request.
/// </summary>
public delegate ValueTask<IResult> BotRequestFilterDelegate(BotRequestFilterContext context);
