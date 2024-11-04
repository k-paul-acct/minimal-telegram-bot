namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     State Machine to manage states of users in Telegram Bot.
/// </summary>
public interface IStateMachine
{
    /// <summary>
    ///     Gets the state of the user with the specified ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>
    ///     The <see cref="ValueTask"/> that represents the asynchronous operation,
    ///     containing the state of the user or null if the user has no state.
    /// </returns>
    ValueTask<State?> GetState(long userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Sets or updates the state of the user with the specified ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="state">The state to be set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask SetState(long userId, State state, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes the state of the user with the specified ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask DropState(long userId, CancellationToken cancellationToken = default);
}
