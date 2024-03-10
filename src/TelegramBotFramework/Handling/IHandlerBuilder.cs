namespace TelegramBotFramework.Handling;

public interface IHandlerBuilder
{
    Handler Handle(HandlerDelegate handlerDelegate);
    Handler Handle(Delegate handlerDelegate);
    Handler Handle(Func<BotRequestContext, Task> func);
    Handler? TryResolveHandler(BotRequestContext ctx);
}