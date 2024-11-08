using System.Text.Json;
using MinimalTelegramBot.StateMachine.Abstractions.Exceptions;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
public static class StateSerializer
{
    private static readonly JsonSerializerOptions _defaultJsonSerializerOptions = JsonSerializerOptions.Default;

    /// <summary>
    /// </summary>
    /// <param name="state"></param>
    /// <param name="typeInfoResolver"></param>
    /// <param name="options"></param>
    /// <typeparam name="TState"></typeparam>
    /// <returns></returns>
    public static SerializedState Serialize<TState>(TState state, IStateTypeInfoResolver typeInfoResolver, StateSerializationOptions? options = null)
    {
        if (!typeInfoResolver.GetInfo(typeof(TState), out var stateEntry))
        {
            throw new StateSerializationException(typeof(TState));
        }

        var json = JsonSerializer.Serialize(state, options?.JsonSerializerOptions ?? _defaultJsonSerializerOptions);

        return new SerializedState(stateEntry, json);
    }

    /// <summary>
    /// </summary>
    /// <param name="serializedState"></param>
    /// <param name="typeInfoResolver"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static TState? Deserialize<TState>(SerializedState serializedState, IStateTypeInfoResolver typeInfoResolver, StateSerializationOptions? options = null)
    {
        if (!typeInfoResolver.GetInfo(serializedState.StateEntry, out var stateType))
        {
            throw new StateSerializationException(serializedState.StateEntry);
        }

        return (TState?)JsonSerializer.Deserialize(serializedState.StateData, stateType, options?.JsonSerializerOptions ?? _defaultJsonSerializerOptions);
    }

    /// <summary>
    /// </summary>
    /// <param name="serializedState"></param>
    /// <param name="typeInfoResolver"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static object? Deserialize(SerializedState serializedState, IStateTypeInfoResolver typeInfoResolver, StateSerializationOptions? options = null)
    {
        if (!typeInfoResolver.GetInfo(serializedState.StateEntry, out var stateType))
        {
            throw new StateSerializationException(serializedState.StateEntry);
        }

        return JsonSerializer.Deserialize(serializedState.StateData, stateType, options?.JsonSerializerOptions ?? _defaultJsonSerializerOptions);
    }
}
