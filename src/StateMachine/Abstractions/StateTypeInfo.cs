namespace MinimalTelegramBot.StateMachine.Abstractions;

// TODO: Docs.
/// <summary>
/// </summary>
/// <param name="StateGroupName"></param>
/// <param name="StateId"></param>
public readonly record struct StateTypeInfo(string StateGroupName, int StateId);
