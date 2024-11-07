using MinimalTelegramBot.Pipeline;

namespace MinimalTelegramBot.StateMachine.Pipes;

internal sealed class StateMachinePipe : IPipe
{
    private readonly IStateMachine _stateMachine;

    public StateMachinePipe(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public async Task InvokeAsync(BotRequestContext context, BotRequestDelegate next)
    {
        var state = await _stateMachine.GetState<object>(context.ChatId);
        context.Data["__State"] = state;
        await next(context);
    }
}
