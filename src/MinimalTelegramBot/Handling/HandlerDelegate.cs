namespace MinimalTelegramBot.Handling;

public delegate Task<IResult> HandlerDelegate(BotRequestContext context);
