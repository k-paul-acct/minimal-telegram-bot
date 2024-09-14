using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Handling.Filters;
using MinimalTelegramBot.Handling.Requirements;
using MinimalTelegramBot.Localization.Filters;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Localization.Extensions;

public static class FilterExtensions
{
    public static TBuilder FilterTextWithLocalizer<TBuilder>(this TBuilder builder, string key)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(key);

        var metadata = new UpdateTypeRequirement(UpdateType.Message);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder.Filter<TBuilder, LocalizedTextFilter>(context => context.Arguments.Add(key));
    }
}
