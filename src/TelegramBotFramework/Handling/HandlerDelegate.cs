namespace TelegramBotFramework.Handling;

public delegate Task HandlerDelegate(BotRequestContext ctx, CancellationToken cancellationToken = default);