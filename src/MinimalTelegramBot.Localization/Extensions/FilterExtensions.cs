using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Localization.Filters;

namespace MinimalTelegramBot.Localization.Extensions;

public static class FilterExtensions
{
    public static IHandlerConventionBuilder FilterTextWithLocalizer(this IHandlerConventionBuilder builder, string key)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(key);

        return handler.Filter<LocalizedTextFilter>();
    }
}
