using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBotFramework.Pipeline;

public class CallbackAutoAnsweringPipe : IPipe
{
    public async Task InvokeAsync(BotRequestContext ctx, BotRequestDelegate next)
    {
        await next(ctx);
        if (ctx.Update.Type == UpdateType.CallbackQuery && ctx.UpdateHandlingStarted)
        {
            await ctx.Client.AnswerCallbackQueryAsync(ctx.Update.CallbackQuery!.Id);
        }
    }
}