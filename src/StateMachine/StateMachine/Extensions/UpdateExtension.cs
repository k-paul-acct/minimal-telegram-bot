using MinimalTelegramBot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.StateMachine.Extensions;

internal static class UpdateExtension
{
    public static StateEntryContext CreateStateEntryContext(this Update update, StateTrackingStrategy stateTrackingStrategy)
    {
        Chat? chat = null;
        var userId = 0L;
        var chatId = 0L;
        var messageThreadId = 0L;
        var isForum = false;
        var chatType = ChatType.Private;

        if ((stateTrackingStrategy & StateTrackingStrategy.DifferentiateUsers) == StateTrackingStrategy.DifferentiateUsers)
        {
            userId = update.GetUserId();
        }

        if ((stateTrackingStrategy & StateTrackingStrategy.DifferentiateChats) == StateTrackingStrategy.DifferentiateChats)
        {
            chat = update.GetChat();
            chatId = chat?.Id ?? 0;
            chatType = chat?.Type ?? ChatType.Private;
        }

        if ((stateTrackingStrategy & StateTrackingStrategy.DifferentiateTopics) == StateTrackingStrategy.DifferentiateTopics)
        {
            chat ??= update.GetChat();
            chatId = chat?.Id ?? 0;
            messageThreadId = update.GetMessageThreadId();
            isForum = chat?.IsForum ?? false;
            chatType = chat?.Type ?? ChatType.Private;
        }

        return new StateEntryContext(stateTrackingStrategy, userId, chatId, messageThreadId, isForum, chatType);
    }
}
