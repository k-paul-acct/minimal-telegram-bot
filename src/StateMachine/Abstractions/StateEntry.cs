namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Represents a some state entry in the <see cref="IStateMachine"/>.
/// </summary>
/// <param name="TypeInfo">The type information of the state.</param>
/// <param name="StateData">The data associated with the state.</param>
public readonly record struct StateEntry(StateTypeInfo TypeInfo, string StateData);
