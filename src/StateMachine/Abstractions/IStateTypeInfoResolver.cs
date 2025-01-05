using System.Diagnostics.CodeAnalysis;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Interface for resolving state type information.
/// </summary>
public interface IStateTypeInfoResolver
{
    /// <summary>
    ///     Tries to get the <see cref="StateTypeInfo"/> for a given type.
    /// </summary>
    /// <param name="type">The type to get information for.</param>
    /// <param name="typeInfo">The <see cref="StateTypeInfo"/> if found.</param>
    /// <returns>True if the information was found; otherwise, false.</returns>
    bool GetInfo(Type type, out StateTypeInfo typeInfo);

    /// <summary>
    ///     Gets the state type for a given <see cref="StateTypeInfo"/>.
    /// </summary>
    /// <param name="typeInfo">The <see cref="StateTypeInfo"/> to get type for.</param>
    /// <param name="stateType">The state type if found.</param>
    /// <returns>True if the state type was found; otherwise, false.</returns>
    bool GetInfo(StateTypeInfo typeInfo, [NotNullWhen(true)] out Type? stateType);
}
