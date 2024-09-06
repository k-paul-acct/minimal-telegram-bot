using MinimalTelegramBot.Handling.Filters;

namespace MinimalTelegramBot.StateMachine.Filters;

internal sealed class StateFilter : IHandlerFilter
{
    private readonly IStateMachine _stateMachine;

    public StateFilter(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public ValueTask<bool> Filter(BotRequestFilterContext context)
    {
        var func = (Func<State?, bool>)context.FilterArguments[0]!;
        var state = _stateMachine.GetState();
        var pass = func(state);
        return ValueTask.FromResult(pass);
    }
}
