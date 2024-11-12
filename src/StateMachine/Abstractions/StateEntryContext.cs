namespace MinimalTelegramBot.StateMachine.Abstractions;

// TODO: Docs.
/// <summary>
/// </summary>
/// <param name="UserId"></param>
/// <param name="ChatId"></param>
/// <param name="MessageThreadId"></param>
public readonly record struct StateEntryContext(long UserId, long ChatId, long MessageThreadId);
