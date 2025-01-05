namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     State Machine abstraction to manage states of users in Telegram Bot.
/// </summary>
public interface IStateMachine
{
    /// <summary>
    ///     Retrieves the state of the user with the specified <see cref="StateEntryContext"/>.
    /// </summary>
    /// <param name="entryContext">The <see cref="StateEntryContext"/> to retrieve state for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <typeparam name="TState">The type of the state to try to deserialize.</typeparam>
    /// <returns>
    ///     The <see cref="ValueTask"/> that represents the asynchronous operation,
    ///     containing the state of the user or null if the user has no state.
    /// </returns>
    ValueTask<TState?> GetState<TState>(StateEntryContext entryContext, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets the state of the user with the specified <see cref="StateEntryContext"/>.
    /// </summary>
    /// <param name="entryContext">The <see cref="StateEntryContext"/> to set state for.</param>
    /// <param name="state">The state to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask SetState<TState>(StateEntryContext entryContext, TState state, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes the state of the user with the specified <see cref="StateEntryContext"/>.
    /// </summary>
    /// <param name="entryContext">The <see cref="StateEntryContext"/> to delete state for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask DropState(StateEntryContext entryContext, CancellationToken cancellationToken = default);
}
