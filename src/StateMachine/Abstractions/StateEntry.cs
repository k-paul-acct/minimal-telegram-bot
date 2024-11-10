namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
/// <param name="TypeInfo"></param>
/// <param name="StateData"></param>
public readonly record struct StateEntry(StateTypeInfo TypeInfo, string StateData);
