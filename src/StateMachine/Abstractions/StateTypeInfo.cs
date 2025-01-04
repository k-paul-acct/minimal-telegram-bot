namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Represents the state type information.
/// </summary>
/// <param name="StateGroupName">The name of the state group.</param>
/// <param name="StateId">The identifier of the state in the group.</param>
public readonly record struct StateTypeInfo(string StateGroupName, int StateId);
