namespace TelegramBotFramework.Pipeline;

public interface IPipe
{
    Task InvokeAsync(BotRequestContext ctx, BotRequestDelegate next);
}