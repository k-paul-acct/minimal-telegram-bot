using System.Diagnostics.CodeAnalysis;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
public interface IStateTypeInfoResolver
{
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
