using System.Text.Json;
using MinimalTelegramBot.StateMachine.Abstractions.Exceptions;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
public static class StateSerializer
{
    /// <summary>
    /// </summary>
    /// <param name="state"></param>
    /// <param name="serializerContext"></param>
    /// <typeparam name="TState"></typeparam>
    /// <returns></returns>
    public static SerializedState Serialize<TState>(TState state, IStateSerializerContext serializerContext)
    {
        var stateType = typeof(TState);

        if (!serializerContext.GetInfo(stateType, out var stateEntry))
        {
            throw new StateSerializationException(stateType);
        }

        var json = JsonSerializer.Serialize(state);

        return new SerializedState(stateEntry, json);
    }

    /// <summary>
    /// </summary>
    /// <param name="serializedState"></param>
    /// <param name="serializerContext"></param>
    /// <returns></returns>
    public static TState? Deserialize<TState>(SerializedState serializedState, IStateSerializerContext serializerContext)
    {
        if (!serializerContext.GetInfo(serializedState.StateEntry, out var stateType) && stateType != typeof(TState))
        {
            throw new StateSerializationException(serializedState.StateEntry);
        }

        return JsonSerializer.Deserialize<TState>(serializedState.SerializedInfo);
    }

    /// <summary>
    /// </summary>
    /// <param name="serializedState"></param>
    /// <param name="serializerContext"></param>
    /// <returns></returns>
    public static object? Deserialize(SerializedState serializedState, IStateSerializerContext serializerContext)
    {
        if (!serializerContext.GetInfo(serializedState.StateEntry, out var stateType))
        {
            throw new StateSerializationException(serializedState.StateEntry);
        }

        return JsonSerializer.Deserialize(serializedState.SerializedInfo, stateType);
    }
}
