using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Handling.Filters;
using MinimalTelegramBot.StateMachine.Filters;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class FilterExtensions
{
    public static HandlerGroupBuilder FilterState<TState>(this HandlerGroupBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.Filter<HandlerGroupBuilder, StateFilter>(context => context.Arguments.Add(typeof(TState)));
    }

    public static HandlerBuilder FilterState<TState>(this HandlerBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.Filter<HandlerBuilder, StateFilter>(context => context.Arguments.Add(typeof(TState)));
    }

    public static HandlerGroupBuilder FilterState<TState>(this HandlerGroupBuilder builder, Func<TState?, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);
        return builder.Filter<HandlerGroupBuilder, StateFilter>(context => context.Arguments.Add(filter));
    }

    public static HandlerBuilder FilterState<TState>(this HandlerBuilder builder, Func<TState?, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);
        return builder.Filter<HandlerBuilder, StateFilter>(context => context.Arguments.Add(filter));
    }
}
