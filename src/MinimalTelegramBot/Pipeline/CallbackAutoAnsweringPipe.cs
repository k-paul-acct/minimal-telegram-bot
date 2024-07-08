using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Pipeline;

public class CallbackAutoAnsweringPipe : IPipe
{
    public async Task InvokeAsync(BotRequestContext ctx, Func<BotRequestContext, Task> next)
    {
        await next(ctx);
        if (ctx.Update.Type == UpdateType.CallbackQuery)
        {
            await ctx.Client.AnswerCallbackQueryAsync(ctx.Update.CallbackQuery!.Id);
        }
    }
}