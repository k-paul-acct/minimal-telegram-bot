using MinimalTelegramBot.StateMachine.Abstractions;

namespace UsageExample.States;

public class EnteringNameStates : State
{
    public static State EnteringText => new EnteringNameStates(1);

    public EnteringNameStates(int stateId) : base(stateId)
    {
    }
}