namespace MinimalTelegramBot.StateMachine.Abstractions;

// TODO: Docs.
/// <summary>
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StateAttribute : Attribute
{
    /// <summary>
    /// </summary>
    public StateAttribute(int stateId)
    {
        StateId = stateId;
    }

    /// <summary>
    /// </summary>
    public int StateId { get; }
}
