using System.Runtime.CompilerServices;
using Telegram.Bot.Types.ReplyMarkups;

namespace MinimalTelegramBot.Results;

internal sealed class ResultHelper
{
    private static readonly Type _messageInlineKeyboardPair = typeof(ValueTuple<string, InlineKeyboardMarkup>);
    private static readonly Type _messageReplyKeyboardPair = typeof(ValueTuple<string, ReplyKeyboardMarkup>);

    public static Func<T, IResult> FromType<T>() where T : notnull
    {
        if (typeof(T) == typeof(string))
        {
            return x => Results.Message((x as string)!);
        }

        if (typeof(T) == _messageReplyKeyboardPair || typeof(T) == _messageInlineKeyboardPair)
        {
            return x =>
            {
                var t = (ITuple)x;
                return Results.Message((string)t[0]!, (IReplyMarkup)t[1]!);
            };
        }

        return x =>
        {
            var message = x.ToString();

            if (string.IsNullOrEmpty(message))
            {
                throw new InvalidOperationException($"A variable of type {typeof(T).FullName} cannot be represented as a non-empty non-null string");
            }

            return Results.Message(message);
        };
    }
}
