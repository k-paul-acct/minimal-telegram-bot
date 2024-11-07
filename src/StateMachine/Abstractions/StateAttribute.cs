namespace MinimalTelegramBot.StateMachine.Abstractions;

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

/*/// <summary>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class StateSerializableAttribute : Attribute
{
    /// <summary>
    /// </summary>
    public StateSerializableAttribute(Type stateSerializableType)
    {
    }
}*/

// [StateSerializable(typeof(FullNameFsm))]
// internal partial class AppStateSerializerContext : StateSerializerContext;
