namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Interface for serializing and deserializing state objects.
/// </summary>
public interface IStateSerializer
{
    /// <summary>
    ///     Serializes the given state object into a <see cref="StateEntry"/>.
    /// </summary>
    /// <param name="state">The state object to serialize.</param>
    /// <typeparam name="TState">The type of the state object.</typeparam>
    /// <returns>A <see cref="StateEntry"/> representing the serialized state.</returns>
    StateEntry Serialize<TState>(TState state);

    /// <summary>
    ///     Deserializes the given <see cref="StateEntry"/> into a state object of type <typeparamref name="TState"/>.
    /// </summary>
    /// <param name="entry">The <see cref="StateEntry"/> to deserialize.</param>
    /// <typeparam name="TState">The type of the state object.</typeparam>
    /// <returns>The deserialized state object of type <typeparamref name="TState"/>.</returns>
    TState? Deserialize<TState>(StateEntry entry);
}
