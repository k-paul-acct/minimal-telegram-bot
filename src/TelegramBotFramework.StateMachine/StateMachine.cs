using TelegramBotFramework.Abstractions;
using TelegramBotFramework.StateMachine.Abstractions;

namespace TelegramBotFramework.StateMachine;

/// <inheritdoc />
public class StateMachine : IStateMachine
{
    private readonly IUserStateRepository _repository;
    private readonly IUserIdProvider<long> _userIdProvider;

    public StateMachine(IUserStateRepository repository, IUserIdProvider<long> userIdProvider)
    {
        _repository = repository;
        _userIdProvider = userIdProvider;
    }

    /// <inheritdoc />
    public void SetState(long userId, State state)
    {
        _repository.SetState(userId, state);
    }

    /// <inheritdoc />
    public void SetState(State state)
    {
        var userId = _userIdProvider.GetUserId();
        SetState(userId, state);
    }

    /// <inheritdoc />
    public State? GetState(long userId)
    {
        return _repository.GetState(userId);
    }

    /// <inheritdoc />
    public State? GetState()
    {
        var userId = _userIdProvider.GetUserId();
        return GetState(userId);
    }

    /// <inheritdoc />
    public bool CheckIfInState(long userId, State state)
    {
        var currentState = GetState(userId);
        return currentState == state;
    }

    /// <inheritdoc />
    public bool CheckIfInState(State state)
    {
        var userId = _userIdProvider.GetUserId();
        return CheckIfInState(userId, state);
    }

    /// <inheritdoc />
    public void DropState(long userId)
    {
        _repository.DeleteState(userId);
    }

    /// <inheritdoc />
    public void DropState()
    {
        var userId = _userIdProvider.GetUserId();
        DropState(userId);
    }
}