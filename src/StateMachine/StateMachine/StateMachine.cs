using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine;

internal sealed class StateMachine : IStateMachine
{
    private readonly StateManagementOptions _stateManagementOptions;

    public StateMachine(IOptions<StateManagementOptions> stateManagementOptions)
    {
        _stateManagementOptions = stateManagementOptions.Value;
    }

    public async ValueTask<TState?> GetState<TState>(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        var repo = _stateManagementOptions.Repository ?? EmptyUserStateRepository.Default;
        var serialized = await repo.GetState(stateEntryContext, cancellationToken);
        if (serialized is null)
        {
            return default;
        }

        var typeInfoResolver = _stateManagementOptions.StateTypeInfoResolver ?? EmptyStateTypeInfoResolver.Default;
        return StateSerializer.Deserialize<TState>(serialized.StateEntry, typeInfoResolver, _stateManagementOptions.StateSerializationOptions);
    }

    public ValueTask SetState<TState>(TState state, StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(state);
        var typeInfoResolver = _stateManagementOptions.StateTypeInfoResolver ?? EmptyStateTypeInfoResolver.Default;
        var stateEntry = StateSerializer.Serialize(state, typeInfoResolver, _stateManagementOptions.StateSerializationOptions);
        var serialized = new SerializedState(stateEntry, stateEntryContext);
        var repo = _stateManagementOptions.Repository ?? EmptyUserStateRepository.Default;
        return repo.SetState(serialized, cancellationToken);
    }

    public ValueTask DropState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        var repo = _stateManagementOptions.Repository ?? EmptyUserStateRepository.Default;
        return repo.DeleteState(stateEntryContext, cancellationToken);
    }

    private sealed class EmptyStateTypeInfoResolver : IStateTypeInfoResolver
    {
        public static readonly EmptyStateTypeInfoResolver Default = new(default, default);

        private readonly StateEntry _stateEntry;
        private readonly Type? _type;

        private EmptyStateTypeInfoResolver(StateEntry stateEntry, Type? type)
        {
            _stateEntry = stateEntry;
            _type = type;
        }

        public bool GetInfo(Type type, out StateEntry stateEntry)
        {
            stateEntry = _stateEntry;
            return false;
        }

        public bool GetInfo(StateEntry stateEntry, [NotNullWhen(true)] out Type? stateType)
        {
            stateType = _type;
            return false;
        }
    }

    private sealed class EmptyUserStateRepository : IUserStateRepository
    {
        public static readonly EmptyUserStateRepository Default = new();

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
}
