namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Interface for managing user states in the state machine.
/// </summary>
public interface IStateRepository
{
    /// <summary>
    ///     Gets the state of the user with the specified ID.
    /// </summary>
    /// <param name="entryContext"></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>
    ///     The <see cref="ValueTask"/> that represents the asynchronous operation,
    ///     containing the state of the user or null if the user has no state.
    /// </returns>
    ValueTask<StateEntry?> GetState(StateEntryContext entryContext, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="entryContext"></param>
    /// <param name="entry"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask SetState(StateEntryContext entryContext, StateEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes the state of the user with the specified ID.
    /// </summary>
    /// <param name="entryContext"></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask DeleteState(StateEntryContext entryContext, CancellationToken cancellationToken = default);
}
