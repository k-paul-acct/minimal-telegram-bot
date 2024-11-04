using MinimalTelegramBot.Handling.Filters;

namespace MinimalTelegramBot.StateMachine.Filters;

internal sealed class StateFilter : IHandlerFilter
{
    private readonly IStateMachine _stateMachine;

    public StateFilter(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async ValueTask<IResult> InvokeAsync(BotRequestFilterContext context, BotRequestFilterDelegate next)
    {
        var predicate = (Func<State?, bool>)context.Arguments[^1]!;
        var state = await _stateMachine.GetState(context.BotRequestContext.ChatId);
        return predicate(state) ? await next(context) : Results.Results.Empty;
    }
}
