namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
public interface IStateSerializer
{
    /// <summary>
    /// </summary>
    /// <param name="state"></param>
    /// <typeparam name="TState"></typeparam>
    /// <returns></returns>
    StateEntry Serialize<TState>(TState state);

    /// <summary>
    /// </summary>
    /// <param name="entry"></param>
    /// <typeparam name="TState"></typeparam>
    /// <returns></returns>
    TState? Deserialize<TState>(StateEntry entry);
}
