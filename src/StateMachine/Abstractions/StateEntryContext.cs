using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
/// <param name="StateTrackingStrategy"></param>
/// <param name="UserId"></param>
/// <param name="ChatId"></param>
/// <param name="MessageThreadId"></param>
/// <param name="IsForum"></param>
/// <param name="ChatType"></param>
public readonly record struct StateEntryContext(
    StateTrackingStrategy StateTrackingStrategy,
    long UserId = 0,
    long ChatId = 0,
    long MessageThreadId = 0,
    bool IsForum = false,
    ChatType ChatType = ChatType.Private);
