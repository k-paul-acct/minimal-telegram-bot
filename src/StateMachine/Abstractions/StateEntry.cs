namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
/// <param name="StateGroupName"></param>
/// <param name="StateId"></param>
/// <param name="StateData"></param>
public readonly record struct StateEntry(string StateGroupName, int StateId, string StateData);
