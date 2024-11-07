using System.Collections.Concurrent;

namespace MinimalTelegramBot.StateMachine;

internal sealed class InMemoryUserStateRepository : IUserStateRepository
{
    private readonly ConcurrentDictionary<long, object> _states = new();

    public ValueTask<TState?> GetState<TState>(long userId, CancellationToken cancellationToken = default)
    {
        _states.TryGetValue(userId, out var state);
        return new ValueTask<TState?>((TState?)state);
    }

    public ValueTask SetState<TState>(long userId, TState state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(state);
        _states.AddOrUpdate(userId, state, (_, _) => state);
        return new ValueTask();
    }

    public ValueTask DeleteState(long userId, CancellationToken cancellationToken = default)
    {
        _states.TryRemove(userId, out _);
        return new ValueTask();
    }
}
