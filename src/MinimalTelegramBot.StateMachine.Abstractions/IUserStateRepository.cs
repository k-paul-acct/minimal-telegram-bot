namespace MinimalTelegramBot.StateMachine.Abstractions;

public interface IUserStateRepository
{
    State? GetState(long userId);
    void SetState(long userId, State state);
    void DeleteState(long userId);
}