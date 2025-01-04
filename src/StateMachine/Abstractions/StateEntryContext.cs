namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Represents a some state context for a distinct user at the chat (message thead) level in the <see cref="IStateMachine"/>.
/// </summary>
/// <param name="UserId">The ID of the user.</param>
/// <param name="ChatId">The ID of the chat the user attached to.</param>
/// <param name="MessageThreadId">The ID of the message thread the user attached to.</param>
public readonly record struct StateEntryContext(long UserId, long ChatId, long MessageThreadId);
