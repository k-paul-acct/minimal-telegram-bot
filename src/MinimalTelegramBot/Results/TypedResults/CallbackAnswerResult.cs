using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Results.TypedResults;

internal sealed class CallbackAnswerResult : IResult
{
    public Task ExecuteAsync(BotRequestContext context)
    {
        if (context.Update.Type != UpdateType.CallbackQuery || context.Data.ContainsKey("__CallbackAnswered"))
        {
            return Task.CompletedTask;
        }

        context.Data["__CallbackAnswered"] = true;

        return context.Client.AnswerCallbackQueryAsync(context.Update.CallbackQuery!.Id);
    }
}
