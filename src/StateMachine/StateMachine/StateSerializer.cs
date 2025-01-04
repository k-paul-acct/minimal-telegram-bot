using System.Text.Json;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine;

internal sealed class StateSerializer : IStateSerializer
{
    private readonly StateSerializerOptions _serializerOptions;
    private readonly IStateTypeInfoResolver _typeInfoResolver;

    public StateSerializer(IStateTypeInfoResolver typeInfoResolver, StateSerializerOptions? serializerOptions = null)
    {
        _typeInfoResolver = typeInfoResolver;
        _serializerOptions = serializerOptions ?? new StateSerializerOptions();
    }

    public StateEntry Serialize<TState>(TState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        var type = typeof(TState);

        if (!_typeInfoResolver.GetInfo(type, out var typeInfo))
        {
            throw new InvalidOperationException($"Cannot find state type info for type {type.FullName ?? type.Name}.");
        }

        var json = JsonSerializer.Serialize(state, _serializerOptions.JsonSerializerOptions);

        return new StateEntry(typeInfo, json);
    }

    public TState? Deserialize<TState>(StateEntry entry)
    {
        if (!_typeInfoResolver.GetInfo(entry.TypeInfo, out var stateType))
        {
            throw new InvalidOperationException($"Cannot find type for state type info {entry.TypeInfo}.");
        }

        return (TState?)JsonSerializer.Deserialize(entry.StateData, stateType, _serializerOptions.JsonSerializerOptions);
    }
}
