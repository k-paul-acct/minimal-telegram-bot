using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Handling.Filters;
using MinimalTelegramBot.Handling.Requirements;
using MinimalTelegramBot.Localization.Filters;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Localization.Extensions;

public static class FilterExtensions
{
    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the <see cref="Message.Text"/> of an incoming <see cref="Message"/> update matches
    ///     the localized string with the given key.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <remarks>
    ///     This filter implicitly implies that the <see cref="Update.Type"/> of an incoming <see cref="Update"/>
    ///     should be a <see cref="UpdateType.Message"/>.
    /// </remarks>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="key">The localization key to retrieve string for comparison.</param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterMessageTextWithLocalizer<TBuilder>(this TBuilder builder, string key)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(key);

        var metadata = new UpdateTypeRequirement(UpdateType.Message);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder.Filter<TBuilder, LocalizedTextFilter>(context => context.Arguments.Add(key));
    }
}
