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
    public static StateEntry Serialize<TState>(TState state, IStateTypeInfoResolver typeInfoResolver, StateSerializationOptions? options = null)
    {
        if (!typeInfoResolver.GetInfo(typeof(TState), out var stateEntry))
        {
            throw new StateSerializationException(typeof(TState));
        }

        var json = JsonSerializer.Serialize(state, options?.JsonSerializerOptions ?? _defaultJsonSerializerOptions);

        return stateEntry with { StateData = json, };
    }

    /// <summary>
    /// </summary>
    /// <param name="stateEntry"></param>
    /// <param name="typeInfoResolver"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static TState? Deserialize<TState>(StateEntry stateEntry, IStateTypeInfoResolver typeInfoResolver, StateSerializationOptions? options = null)
    {
        if (!typeInfoResolver.GetInfo(stateEntry, out var stateType))
        {
            throw new StateSerializationException(stateEntry);
        }

        return (TState?)JsonSerializer.Deserialize(stateEntry.StateData, stateType, options?.JsonSerializerOptions ?? _defaultJsonSerializerOptions);
    }

    /// <summary>
    /// </summary>
    /// <param name="stateEntry"></param>
    /// <param name="typeInfoResolver"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static object? Deserialize(StateEntry stateEntry, IStateTypeInfoResolver typeInfoResolver, StateSerializationOptions? options = null)
    {
        if (!typeInfoResolver.GetInfo(stateEntry, out var stateType))
        {
            throw new StateSerializationException(stateEntry);
        }

        return JsonSerializer.Deserialize(stateEntry.StateData, stateType, options?.JsonSerializerOptions ?? _defaultJsonSerializerOptions);
    }
}
