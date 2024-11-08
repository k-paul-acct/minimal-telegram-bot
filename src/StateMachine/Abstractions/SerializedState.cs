namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
/// <param name="StateEntry"></param>
/// <param name="StateData"></param>
public readonly record struct SerializedState(StateEntry StateEntry, string StateData);
