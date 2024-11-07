using MinimalTelegramBot.StateMachine.Abstractions;

namespace UsageExample.StateMachine.EntityFrameworkCore;

[StateGroup("FullNameFsm")]
public static class FullNameFsm
{
    [State(1)]
    public class EnteringFirstNameState;

    [State(2)]
    public class EnteringLastNameState
    {
        public required string FirstName { get; init; }
    }
}
