using MinimalTelegramBot.Handling.Filters;

namespace MinimalTelegramBot.StateMachine.Filters;

internal sealed class StateFilter : IHandlerFilter
{
    private readonly IStateMachine _stateMachine;

    public StateFilter(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public ValueTask<IResult> InvokeAsync(BotRequestFilterContext context, BotRequestFilterDelegate next)
    {
        var predicate = (Func<State?, bool>)context.Arguments[^1]!;
        var state = _stateMachine.GetState();
        return predicate(state) ? next(context) : new ValueTask<IResult>(Results.Results.Empty);
    }
}
