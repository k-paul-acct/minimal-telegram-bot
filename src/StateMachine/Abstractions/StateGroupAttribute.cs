namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Attribute to define a group of states for some custom workflow.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StateGroupAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="StateGroupAttribute"/> with the specified state group name.
    /// </summary>
    /// <param name="stateGroupName">The name of the state group.</param>
    public StateGroupAttribute(string stateGroupName)
    {
        StateGroupName = stateGroupName;
    }

    /// <summary>
    ///     Gets the name of the state group.
    /// </summary>
    public string StateGroupName { get; }
}
