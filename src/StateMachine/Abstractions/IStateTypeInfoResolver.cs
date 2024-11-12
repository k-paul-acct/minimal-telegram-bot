using System.Diagnostics.CodeAnalysis;

namespace MinimalTelegramBot.StateMachine.Abstractions;

// TODO: Docs.
/// <summary>
/// </summary>
public interface IStateTypeInfoResolver
{
    /// <summary>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="typeInfo"></param>
    /// <returns></returns>
    bool GetInfo(Type type, out StateTypeInfo typeInfo);

    /// <summary>
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <param name="stateType"></param>
    /// <returns></returns>
    bool GetInfo(StateTypeInfo typeInfo, [NotNullWhen(true)] out Type? stateType);
}
