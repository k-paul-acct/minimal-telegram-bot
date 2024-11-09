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

    public async ValueTask<TState?> GetState<TState>(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        var serialized = await _repository.GetState(stateEntryContext, cancellationToken);
        return serialized is null
            ? default
            : _serializer.Deserialize<TState>(serialized.StateEntry);
    }

    public ValueTask SetState<TState>(TState state, StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(state);
        var stateEntry = _serializer.Serialize(state);
        var serialized = new SerializedState(stateEntry, stateEntryContext);
        return _repository.SetState(serialized, cancellationToken);
    }

    public ValueTask DropState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        return _repository.DeleteState(stateEntryContext, cancellationToken);
    }

    private sealed class EmptyStateRepository : IStateRepository
    {
        public static readonly EmptyStateRepository Default = new();

        public ValueTask<SerializedState?> GetState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
        {
            return default;
        }

        public ValueTask SetState(SerializedState serializedState, CancellationToken cancellationToken = default)
        {
            return default;
        }

        public ValueTask DeleteState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
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

        public TState? Deserialize<TState>(StateEntry stateEntry)
        {
            return default;
        }
    }
}
