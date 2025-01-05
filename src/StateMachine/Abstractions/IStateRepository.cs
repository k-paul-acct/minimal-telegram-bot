namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Interface for managing user states and its persistence in the state machine.
/// </summary>
public interface IStateRepository
{
    /// <summary>
    ///     Retrieves the state of the user with the specified <see cref="StateEntryContext"/>.
    /// </summary>
    /// <param name="entryContext">The <see cref="StateEntryContext"/> to retrieve state for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>
    ///     The <see cref="ValueTask"/> that represents the asynchronous operation,
    ///     containing the state of the user or null if the user has no state.
    /// </returns>
    ValueTask<StateEntry?> GetState(StateEntryContext entryContext, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets the state of the user with the specified <see cref="StateEntryContext"/>.
    /// </summary>
    /// <param name="entryContext">The <see cref="StateEntryContext"/> to set state for.</param>
    /// <param name="entry">The <see cref="StateEntry"/> to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask SetState(StateEntryContext entryContext, StateEntry entry, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes the state of the user with the specified <see cref="StateEntryContext"/>.
    /// </summary>
    /// <param name="entryContext">The <see cref="StateEntryContext"/> to delete state for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask DeleteState(StateEntryContext entryContext, CancellationToken cancellationToken = default);
}
