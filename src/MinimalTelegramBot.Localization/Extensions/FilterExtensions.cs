using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Localization.Filters;

namespace MinimalTelegramBot.Localization.Extensions;

public static class FilterExtensions
{
    public static Handler FilterTextWithLocalizer(this Handler handler, string key)
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentNullException.ThrowIfNull(key);

        return handler.Filter<LocalizedTextFilter>([key,]);
    }
}