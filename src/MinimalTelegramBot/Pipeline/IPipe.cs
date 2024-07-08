namespace MinimalTelegramBot.Pipeline;

public interface IPipe
{
    Task InvokeAsync(BotRequestContext ctx, Func<BotRequestContext, Task> next);
}