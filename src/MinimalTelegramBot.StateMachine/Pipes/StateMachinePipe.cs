using MinimalTelegramBot.Pipeline;

namespace MinimalTelegramBot.StateMachine.Pipes;

internal sealed class StateMachinePipe : IPipe
{
    private readonly IStateMachine _stateMachine;

    public StateMachinePipe(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public Task InvokeAsync(BotRequestContext ctx, BotRequestDelegate next)
    {
        var state = _stateMachine.GetState(ctx.ChatId);
        ctx.UserState = state;
        return next(ctx);
    }
}
