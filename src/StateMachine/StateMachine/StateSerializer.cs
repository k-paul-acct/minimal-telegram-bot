using System.Text.Json;
using MinimalTelegramBot.StateMachine.Abstractions;
using MinimalTelegramBot.StateMachine.Abstractions.Exceptions;

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

        if (!_typeInfoResolver.GetInfo(typeof(TState), out var typeInfo))
        {
            throw new StateSerializationException(typeof(TState));
        }

        var json = JsonSerializer.Serialize(state, _serializerOptions.JsonSerializerOptions);

        return new StateEntry(typeInfo, json);
    }

    public TState? Deserialize<TState>(StateEntry entry)
    {
        if (!_typeInfoResolver.GetInfo(entry.TypeInfo, out var stateType))
        {
            throw new StateSerializationException(entry);
        }

        return (TState?)JsonSerializer.Deserialize(entry.StateData, stateType, _serializerOptions.JsonSerializerOptions);
    }
}
