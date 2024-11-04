using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Pipeline;

internal sealed class CallbackAutoAnsweringPipe : IPipe
{
    public async Task InvokeAsync(BotRequestContext context, BotRequestDelegate next)
    {
        await next(context);
        if (context.Update.Type == UpdateType.CallbackQuery && !context.Data.ContainsKey("__CallbackAnswered"))
        {
            context.Data["__CallbackAnswered"] = true;
            await context.Client.AnswerCallbackQuery(context.Update.CallbackQuery!.Id);
        }
    }
}
