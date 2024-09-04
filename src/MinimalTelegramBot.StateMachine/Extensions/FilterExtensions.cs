using MinimalTelegramBot.Handling;
using MinimalTelegramBot.StateMachine.Filters;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class FilterExtensions
{
    public static Handler FilterState(this Handler handler, State state)
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentNullException.ThrowIfNull(state);

        return handler.FilterState(userState => userState == state);
    }

    public static Handler FilterState(this Handler handler, Func<State?, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(handler);
        ArgumentNullException.ThrowIfNull(filter);

        return handler.Filter<StateFilter>([filter,]);
    }
}