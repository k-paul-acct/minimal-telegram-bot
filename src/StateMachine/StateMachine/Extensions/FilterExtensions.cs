using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Handling.Filters;
using MinimalTelegramBot.Results;
using MinimalTelegramBot.StateMachine.Abstractions;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.StateMachine.Extensions;

/// <summary>
///     FilterExtensions.
/// </summary>
public static class FilterExtensions
{
    /// <summary>
    ///     Adds a filter to the handler group builder that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the state for current <see cref="StateEntryContext"/> is <typeparamref name="TState"/>.
    ///     If not, the <see cref="MinimalTelegramBot.Results.Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <typeparam name="TState">Required type of the current state.</typeparam>
    /// <param name="builder">The handler group builder.</param>
    /// <returns>The handler group builder that can be used for further customization.</returns>
    public static HandlerGroupBuilder FilterState<TState>(this HandlerGroupBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.Filter((context, next) =>
        {
            var state = context.BotRequestContext.Data["__State"];
            return state is not null && state.GetType() == typeof(TState)
                ? next(context)
                : new ValueTask<IResult>(Results.Results.Empty);
        });
    }

    /// <summary>
    ///     Adds a filter to the handler builder that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the state for current <see cref="StateEntryContext"/> is <typeparamref name="TState"/>.
    ///     If not, the <see cref="MinimalTelegramBot.Results.Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <typeparam name="TState">Required type of the current state.</typeparam>
    /// <param name="builder">The handler builder.</param>
    /// <returns>The handler builder that can be used for further customization.</returns>
    public static HandlerBuilder FilterState<TState>(this HandlerBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.Filter((context, next) =>
        {
            var state = context.BotRequestContext.Data["__State"];
            return state is not null && state.GetType() == typeof(TState)
                ? next(context)
                : new ValueTask<IResult>(Results.Results.Empty);
        });
    }

    /// <summary>
    ///     Adds a filter to the handler group builder that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the state for current <see cref="StateEntryContext"/> satisfies
    ///     the specified filter.
    ///     If not, the <see cref="MinimalTelegramBot.Results.Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <typeparam name="TState">Required type of the current state.</typeparam>
    /// <param name="builder">The handler group builder.</param>
    /// <param name="filter">The predicate to check the state for current <see cref="StateEntryContext"/>.</param>
    /// <returns>The handler group builder that can be used for further customization.</returns>
    public static HandlerGroupBuilder FilterState<TState>(this HandlerGroupBuilder builder, Func<TState?, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.Filter((context, next) =>
        {
            var state = (TState?)context.BotRequestContext.Data["__State"];
            return filter(state) ? next(context) : new ValueTask<IResult>(Results.Results.Empty);
        });
    }

    /// <summary>
    ///     Adds a filter to the handler builder that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the state for current <see cref="StateEntryContext"/> satisfies
    ///     the specified filter.
    ///     If not, the <see cref="MinimalTelegramBot.Results.Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <typeparam name="TState">Required type of the current state.</typeparam>
    /// <param name="builder">The handler builder.</param>
    /// <param name="filter">The predicate to check the state for current <see cref="StateEntryContext"/>.</param>
    /// <returns>The handler builder that can be used for further customization.</returns>
    public static HandlerBuilder FilterState<TState>(this HandlerBuilder builder, Func<TState?, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.Filter((context, next) =>
        {
            var state = (TState?)context.BotRequestContext.Data["__State"];
            return filter(state) ? next(context) : new ValueTask<IResult>(Results.Results.Empty);
        });
    }
}
