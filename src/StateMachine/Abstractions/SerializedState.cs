namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
/// <param name="StateEntry"></param>
/// <param name="StateEntryContext"></param>
public sealed record SerializedState(StateEntry StateEntry, StateEntryContext StateEntryContext);
