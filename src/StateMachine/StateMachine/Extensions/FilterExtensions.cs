using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Handling.Filters;
using MinimalTelegramBot.Results;

namespace MinimalTelegramBot.StateMachine.Extensions;

// TODO: Docs.
public static class FilterExtensions
{
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
