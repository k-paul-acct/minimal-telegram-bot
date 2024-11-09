using System.Text.Json;
using MinimalTelegramBot.StateMachine.Abstractions;
using MinimalTelegramBot.StateMachine.Abstractions.Exceptions;

namespace MinimalTelegramBot.StateMachine;

internal sealed class StateSerializer : IStateSerializer
{
    private readonly StateSerializationOptions _serializationOptions;
    private readonly IStateTypeInfoResolver _typeInfoResolver;

    public StateSerializer(IStateTypeInfoResolver typeInfoResolver, StateSerializationOptions? serializationOptions = null)
    {
        _typeInfoResolver = typeInfoResolver;
        _serializationOptions = serializationOptions ?? new StateSerializationOptions();
    }

    public StateEntry Serialize<TState>(TState state)
    {
        if (!_typeInfoResolver.GetInfo(typeof(TState), out var stateEntry))
        {
            throw new StateSerializationException(typeof(TState));
        }

        var json = JsonSerializer.Serialize(state, _serializationOptions.JsonSerializerOptions);

        return stateEntry with { StateData = json, };
    }

    public TState? Deserialize<TState>(StateEntry stateEntry)
    {
        if (!_typeInfoResolver.GetInfo(stateEntry, out var stateType))
        {
            throw new StateSerializationException(stateEntry);
        }

        return (TState?)JsonSerializer.Deserialize(stateEntry.StateData, stateType, _serializationOptions.JsonSerializerOptions);
    }
}
