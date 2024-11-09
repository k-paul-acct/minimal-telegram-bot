namespace MinimalTelegramBot.StateMachine.Abstractions.Exceptions;

/// <summary>
/// </summary>
public sealed class StateSerializationException : Exception
{
    /// <summary>
    /// </summary>
    /// <param name="stateType"></param>
    public StateSerializationException(Type stateType)
        : base($"Cannot find proper state entry for state type {stateType.FullName ?? stateType.Name}.")
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="stateEntry"></param>
    public StateSerializationException(StateEntry stateEntry)
        : base($"Cannot find proper state type for state entry {stateEntry}.")
    {
    }
}