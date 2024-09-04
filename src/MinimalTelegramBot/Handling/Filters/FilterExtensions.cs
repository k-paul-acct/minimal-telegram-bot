using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling.Filters;

public static class FilterExtensions
{
    public static Handler FilterText(this Handler handler, Func<string, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentNullException.ThrowIfNull(filter);

        return handler.Filter(ctx => ctx.BotRequestContext.MessageText is not null && filter(ctx.BotRequestContext.MessageText));
    }

    public static Handler FilterCommand(this Handler handler, string command)
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentNullException.ThrowIfNull(command);

        return handler.Filter(ctx =>
        {
            if (ctx.BotRequestContext.MessageText is null)
            {
                return false;
            }

            var span = ctx.BotRequestContext.MessageText.AsSpan();

            if (span.Length < 2 || span[0] != '/')
            {
                return false;
            }

            var commandEnd = 1;

            foreach (var letter in span[1..])
            {
                var uLetter = (uint)letter;

                if (uLetter - 'a' > 'z' - 'a' && uLetter - '0' > '9' - '0' && uLetter != '_')
                {
                    break;
                }

                commandEnd += 1;
            }

            if (commandEnd < 2)
            {
                return false;
            }

            return span[..commandEnd].SequenceEqual(command);
        });
    }

    public static Handler FilterCallbackData(this Handler handler, Func<string, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentNullException.ThrowIfNull(filter);

        return handler.Filter(ctx => ctx.BotRequestContext.CallbackData is not null && filter(ctx.BotRequestContext.CallbackData));
    }

    public static Handler FilterUpdateType(this Handler handler, UpdateType updateType)
    {
        ArgumentNullException.ThrowIfNull(handler);

        return handler.Filter(ctx => ctx.BotRequestContext.Update.Type == updateType);
    }

    public static Handler FilterUpdateType(this Handler handler, Func<UpdateType, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentNullException.ThrowIfNull(filter);

        return handler.Filter(ctx => filter(ctx.BotRequestContext.Update.Type));
    }
}