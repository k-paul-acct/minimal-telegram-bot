namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Interface for managing user states in the state machine.
/// </summary>
public interface IUserStateRepository
{
    /// <summary>
    ///     Gets the state of the user with the specified ID.
    /// </summary>
    /// <param name="stateEntryContext"></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>
    ///     The <see cref="ValueTask"/> that represents the asynchronous operation,
    ///     containing the state of the user or null if the user has no state.
    /// </returns>
    ValueTask<SerializedState?> GetState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets or updates the state of the user with the specified ID.
    /// </summary>
    /// <param name="serializedState"></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask SetState(SerializedState serializedState, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes the state of the user with the specified ID.
    /// </summary>
    /// <param name="stateEntryContext"></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask DeleteState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default);
}
