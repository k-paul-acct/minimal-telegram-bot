using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Pipeline.TypedPipes;

internal sealed class CallbackAutoAnsweringPipe : IPipe
{
    public async Task InvokeAsync(BotRequestContext ctx, BotRequestDelegate next)
    {
        await next(ctx);
        if (ctx.Update.Type == UpdateType.CallbackQuery && !ctx.Data.ContainsKey("__CallbackAnswered"))
        {
            ctx.Data["__CallbackAnswered"] = true;
            await ctx.Client.AnswerCallbackQueryAsync(ctx.Update.CallbackQuery!.Id);
        }
    }
}
