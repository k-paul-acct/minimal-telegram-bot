namespace MinimalTelegramBot.Handling.Filters;

public delegate ValueTask<IResult> BotRequestFilterDelegate(BotRequestFilterContext context);
