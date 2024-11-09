namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     State Machine to manage states of users in Telegram Bot.
/// </summary>
public interface IStateMachine
{
    /// <summary>
    /// </summary>
    /// <param name="stateEntryContext"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TState"></typeparam>
    /// <returns></returns>
    ValueTask<TState?> GetState<TState>(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="state"></param>
    /// <param name="stateEntryContext"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TState"></typeparam>
    /// <returns></returns>
    ValueTask SetState<TState>(TState state, StateEntryContext stateEntryContext, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes the state of the user with the specified ID.
    /// </summary>
    /// <param name="stateEntryContext"></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    ValueTask DropState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default);
}
