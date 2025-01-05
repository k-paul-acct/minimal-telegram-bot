using MinimalTelegramBot.Extensions;
using MinimalTelegramBot.StateMachine.Abstractions;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.StateMachine.Extensions;

internal static class UpdateExtension
{
    public static StateEntryContext CreateStateEntryContext(this Update update, StateTrackingStrategy trackingStrategy)
    {
        Chat? chat = null;
        var userId = 0L;
        var chatId = 0L;
        var messageThreadId = 0L;

        if ((trackingStrategy & StateTrackingStrategy.DifferentiateUsers) == StateTrackingStrategy.DifferentiateUsers)
        {
            userId = update.GetUserId();
        }

        if ((trackingStrategy & StateTrackingStrategy.DifferentiateChats) == StateTrackingStrategy.DifferentiateChats)
        {
            chat = update.GetChat();
            chatId = chat?.Id ?? 0;
        }

        if ((trackingStrategy & StateTrackingStrategy.DifferentiateTopics) == StateTrackingStrategy.DifferentiateTopics)
        {
            chat ??= update.GetChat();
            chatId = chat?.Id ?? 0;
            messageThreadId = update.GetMessageThreadId();
        }

        return new StateEntryContext(userId, chatId, messageThreadId);
    }
}
