using System.Collections.Concurrent;

namespace MinimalTelegramBot.StateMachine;

internal sealed class InMemoryUserStateRepository : IUserStateRepository
{
    private readonly ConcurrentDictionary<long, State> _states = new();

    public ValueTask<State?> GetState(long userId, CancellationToken cancellationToken = default)
    {
        _states.TryGetValue(userId, out var state);
        return new ValueTask<State?>(state);
    }

    public ValueTask SetState(long userId, State state, CancellationToken cancellationToken = default)
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
