namespace MinimalTelegramBot.StateMachine.Abstractions;

// TODO: Docs.
/// <summary>
/// </summary>
/// <param name="TypeInfo"></param>
/// <param name="StateData"></param>
public readonly record struct StateEntry(StateTypeInfo TypeInfo, string StateData);
