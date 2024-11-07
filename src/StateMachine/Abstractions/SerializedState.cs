namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
/// <param name="StateEntry"></param>
/// <param name="SerializedInfo"></param>
public readonly record struct SerializedState(StateEntry StateEntry, string SerializedInfo);
