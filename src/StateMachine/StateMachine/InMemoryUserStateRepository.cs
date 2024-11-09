using System.Collections.Concurrent;

namespace MinimalTelegramBot.StateMachine;

internal sealed class InMemoryUserStateRepository : IUserStateRepository
{
    private readonly ConcurrentDictionary<StateEntryContext, SerializedState> _states = new();

    public ValueTask<SerializedState?> GetState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        _states.TryGetValue(stateEntryContext, out var state);
        return new ValueTask<SerializedState?>(state);
    }

    public ValueTask SetState(SerializedState serializedState, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(serializedState);
        _states.AddOrUpdate(serializedState.StateEntryContext, serializedState, (_, _) => serializedState);
        return new ValueTask();
    }

    public ValueTask DeleteState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        _states.TryRemove(stateEntryContext, out _);
        return new ValueTask();
    }
}
