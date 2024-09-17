namespace MinimalTelegramBot.Pipeline;

public interface IPipe
{
    Task InvokeAsync(BotRequestContext context, BotRequestDelegate next);
}
