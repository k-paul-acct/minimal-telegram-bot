namespace MinimalTelegramBot.StateMachine.Abstractions;

// TODO: Docs.
/// <summary>
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StateGroupAttribute : Attribute
{
    /// <summary>
    /// </summary>
    public StateGroupAttribute(string stateGroupName)
    {
        StateGroupName = stateGroupName;
    }

    /// <summary>
    /// </summary>
    public string StateGroupName { get; }
}
