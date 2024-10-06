namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     State Machine to manage states of users in Telegram Bot.
/// </summary>
public interface IStateMachine
{
    /// <summary>
    ///     Set or update state for user with given ID.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="state">State to set.</param>
    void SetState(long userId, State state);

    /// <summary>
    ///     Set or update state for current user in bot request pipeline.
    /// </summary>
    /// <param name="state">State to set.</param>
    void SetState(State state);

    /// <summary>
    ///     Get state of user with given ID.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <returns>State if was set before or null if user has no state.</returns>
    State? GetState(long userId);

    /// <summary>
    ///     Get state of current user in bot request pipeline.
    /// </summary>
    /// <returns>State if was set before or null if user has no state.</returns>
    State? GetState();

    /// <summary>
    ///     Checks if user with given ID is in the given state.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="state">State to check against of.</param>
    /// <returns>True if user has state and that state matches the given state, false otherwise.</returns>
    bool CheckIfInState(long userId, State state);

    /// <summary>
    ///     Checks if current user in bot request pipeline is in the given state.
    /// </summary>
    /// <param name="state">State to check against of.</param>
    /// <returns>True if user has state and that state matches the given state, false otherwise.</returns>
    bool CheckIfInState(State state);

    /// <summary>
    ///     Deletes state of user with the given ID or, equally, sets the state to null.
    /// </summary>
    /// <param name="userId">User ID.</param>
    void DropState(long userId);

    /// <summary>
    ///     Deletes state of current user in bot request pipeline or, equally, sets the state to null.
    /// </summary>
    void DropState();
}
