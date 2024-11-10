using Microsoft.Extensions.Options;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine;

internal sealed class StateMachine : IStateMachine
{
    private readonly IStateRepository _repository;
    private readonly IStateSerializer _serializer;

    public StateMachine(IOptions<StateManagementOptions> stateManagementOptions)
    {
        _repository = stateManagementOptions.Value.Repository ?? EmptyStateRepository.Default;
        _serializer = stateManagementOptions.Value.Serializer ?? EmptyStateSerializer.Default;
    }

    public async ValueTask<TState?> GetState<TState>(StateEntryContext entryContext, CancellationToken cancellationToken = default)
    {
        var entry = await _repository.GetState(entryContext, cancellationToken);
        return entry is null ? default : _serializer.Deserialize<TState>(entry.Value);
    }

    public ValueTask SetState<TState>(StateEntryContext entryContext, TState state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(state);
        var entry = _serializer.Serialize(state);
        return _repository.SetState(entryContext, entry, cancellationToken);
    }

    public ValueTask DropState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteState(entryContext, cancellationToken);
    }

    private sealed class EmptyStateRepository : IStateRepository
    {
        public static readonly EmptyStateRepository Default = new();

        public ValueTask<StateEntry?> GetState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
        {
            return default;
        }

        public ValueTask SetState(StateEntryContext entryContext, StateEntry entry, CancellationToken cancellationToken = default)
        {
            return default;
        }

        public ValueTask DeleteState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
        {
            return default;
        }
    }

    private sealed class EmptyStateSerializer : IStateSerializer
    {
        public static readonly EmptyStateSerializer Default = new();

        public StateEntry Serialize<TState>(TState state)
        {
            return default;
        }

        public TState? Deserialize<TState>(StateEntry entry)
        {
            return default;
        }
    }
}
