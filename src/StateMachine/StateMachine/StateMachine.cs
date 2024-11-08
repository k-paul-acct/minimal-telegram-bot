using Microsoft.Extensions.Options;

namespace MinimalTelegramBot.StateMachine;

internal sealed class StateMachine : IStateMachine
{
    private readonly IUserStateRepository _userStateRepository;

    public StateMachine(IOptions<StateManagementOptions> stateManagementOptions)
    {
        _userStateRepository = stateManagementOptions.Value.Repository!;
    }

    public ValueTask<TState?> GetState<TState>(long userId, CancellationToken cancellationToken = default)
    {
        return _userStateRepository.GetState<TState>(userId, cancellationToken);
    }

    public ValueTask SetState<TState>(long userId, TState state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(state);
        return _userStateRepository.SetState(userId, state, cancellationToken);
    }

    public ValueTask DropState(long userId, CancellationToken cancellationToken = default)
    {
        return _userStateRepository.DeleteState(userId, cancellationToken);
    }
}
