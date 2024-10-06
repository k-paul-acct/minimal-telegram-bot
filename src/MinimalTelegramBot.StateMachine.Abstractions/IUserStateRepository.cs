namespace MinimalTelegramBot.StateMachine.Abstractions;

public interface IUserStateRepository
{
    State? GetState(long userId);
    void SetState(long userId, State state);
    void DeleteState(long userId);
    Task<State?> GetStateAsync(long userId, CancellationToken cancellationToken = default);
    Task SetStateAsync(long userId, State state, CancellationToken cancellationToken = default);
    Task DeleteStateAsync(long userId, CancellationToken cancellationToken = default);
}
