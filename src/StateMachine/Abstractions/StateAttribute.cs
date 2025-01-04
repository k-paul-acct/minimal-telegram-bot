namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Attribute to mark nested class as the possible state in the state group marked with <see cref="StateGroupAttribute"/>.
///     Attaches the state ID to the state class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class StateAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="StateAttribute"/> with the specified state ID.
    /// </summary>
    public StateAttribute(int stateId)
    {
        StateId = stateId;
    }

    /// <summary>
    ///     Gets the state ID associated with this attribute.
    /// </summary>
    public int StateId { get; }
}
