namespace MinimalTelegramBot.StateMachine;

/// <inheritdoc />
internal sealed class StateMachine : IStateMachine
{
    private readonly IUserStateRepository _repository;
    private readonly BotRequestContext _context;

    public StateMachine(IUserStateRepository repository, IBotRequestContextAccessor contextAccessor)
    {
        _repository = repository;
        _context = contextAccessor.BotRequestContext ??
                   throw new Exception($"No current context in {nameof(IBotRequestContextAccessor)}");
    }

    /// <inheritdoc />
    public void SetState(long userId, State state)
    {
        _repository.SetState(userId, state);
    }

    /// <inheritdoc />
    public void SetState(State state)
    {
        var userId = _context.ChatId;
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
        var userId = _context.ChatId;
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
        var userId = _context.ChatId;
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
        var userId = _context.ChatId;
        DropState(userId);
    }
}
