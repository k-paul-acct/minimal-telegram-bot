using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
public interface IStateSerializerContext
{
    /// <summary>
    /// </summary>
    public JsonSerializerOptions? JsonSerializerOptions { get; }

    /// <summary>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="stateEntry"></param>
    /// <returns></returns>
    bool GetInfo(Type type, out StateEntry stateEntry);

    /// <summary>
    /// </summary>
    /// <param name="stateEntry"></param>
    /// <param name="stateType"></param>
    /// <returns></returns>
    bool GetInfo(StateEntry stateEntry, [NotNullWhen(true)] out Type? stateType);
}
