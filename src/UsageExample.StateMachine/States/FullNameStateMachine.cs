using MinimalTelegramBot.StateMachine.Abstractions;

namespace UsageExample.StateMachine.States;

public sealed class FullNameStateMachine : State
{
    private FullNameStateMachine(int stateId) : base(stateId)
    {
    }

    public static FullNameStateMachine EnteringFirstName => new(1);
    public static FullNameStateMachine EnteringLastName => new(2);
}
