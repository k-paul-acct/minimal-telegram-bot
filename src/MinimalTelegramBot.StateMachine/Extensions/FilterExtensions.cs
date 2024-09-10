using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Handling.Filters;
using MinimalTelegramBot.StateMachine.Filters;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class FilterExtensions
{
    public static TBuilder FilterState<TBuilder>(this TBuilder builder, State state)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(state);

        return builder.FilterState(userState => userState == state);
    }

    public static TBuilder FilterState<TBuilder>(this TBuilder builder, Func<State?, bool> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.Filter<TBuilder, StateFilter>(context => context.Arguments.Add(filter));
    }
}
