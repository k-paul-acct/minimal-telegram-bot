namespace MinimalTelegramBot.Handling;

public interface IHandlerBuilder
{
    Handler Handle(Delegate handlerDelegate);
    Handler Handle(Func<BotRequestContext, Task> func);
    Handler? TryResolveHandler(BotRequestContext ctx);
}