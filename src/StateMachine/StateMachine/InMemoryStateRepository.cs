using System.Collections.Concurrent;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine;

internal sealed class InMemoryStateRepository : IStateRepository
{
    private readonly ConcurrentDictionary<StateEntryContext, StateEntry> _states = new();

    public ValueTask<StateEntry?> GetState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
    {
        return _states.TryGetValue(entryContext, out var state)
            ? new ValueTask<StateEntry?>(state)
            : new ValueTask<StateEntry?>(new StateEntry?());
    }

    public ValueTask SetState(StateEntryContext entryContext, StateEntry entry, CancellationToken cancellationToken = default)
    {
        _states.AddOrUpdate(entryContext, entry, (_, _) => entry);
        return new ValueTask();
    }

    public ValueTask DeleteState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
    {
        _states.TryRemove(entryContext, out _);
        return new ValueTask();
    }
}
