namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Interface for managing user states in the state machine.
/// </summary>
public interface IUserStateRepository
{
    /// <summary>
    ///     Retrieves the state associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose state is to be retrieved.</param>
    /// <returns>The state of the user, or null if no state is found.</returns>
    State? GetState(long userId);

    /// <summary>
    ///     Sets the state associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose state is to be set.</param>
    /// <param name="state">The state to be set for the user.</param>
    void SetState(long userId, State state);

    /// <summary>
    ///     Deletes the state associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user whose state is to be deleted.</param>
    void DeleteState(long userId);
    Task<State?> GetStateAsync(long userId, CancellationToken cancellationToken = default);
    Task SetStateAsync(long userId, State state, CancellationToken cancellationToken = default);
    Task DeleteStateAsync(long userId, CancellationToken cancellationToken = default);
}
