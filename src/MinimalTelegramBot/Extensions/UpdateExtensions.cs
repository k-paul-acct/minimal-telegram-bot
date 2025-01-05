using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Extensions;

/// <summary>
///     UpdateExtensions.
/// </summary>
public static class UpdateExtensions
{
    /// <summary>
    ///     Gets the ID of the <see cref="User"/> from the <see cref="Update"/>.
    /// </summary>
    /// <param name="update">The <see cref="Update"/>.</param>
    /// <returns>The ID of the <see cref="User"/> or 0 if ID cannot be retrieved.</returns>
    public static long GetUserId(this Update update)
    {
        ArgumentNullException.ThrowIfNull(update);

        return update.Message?.From?.Id ??
               update.CallbackQuery?.From.Id ??
               update.EditedMessage?.From?.Id ??
               update.ChannelPost?.From?.Id ??
               update.EditedChannelPost?.From?.Id ??
               update.MessageReaction?.User?.Id ??
               update.ChatJoinRequest?.From.Id ??
               update.MyChatMember?.From.Id ??
               update.ChatMember?.From.Id ??
               0;
    }

    /// <summary>
    ///     Gets the <see cref="Chat"/> from the <see cref="Update"/>.
    /// </summary>
    /// <param name="update">The <see cref="Update"/>.</param>
    /// <returns>The <see cref="Chat"/> or null if <see cref="Chat"/> cannot be retrieved.</returns>
    public static Chat? GetChat(this Update update)
    {
        ArgumentNullException.ThrowIfNull(update);

        return update.Message?.Chat ??
               update.CallbackQuery?.Message?.Chat ??
               update.EditedMessage?.Chat ??
               update.ChannelPost?.Chat ??
               update.EditedChannelPost?.Chat ??
               update.MessageReaction?.Chat ??
               update.MessageReactionCount?.Chat ??
               update.ChatBoost?.Chat ??
               update.RemovedChatBoost?.Chat ??
               update.ChatJoinRequest?.Chat ??
               update.MyChatMember?.Chat ??
               update.ChatMember?.Chat ??
               null;
    }

    /// <summary>
    ///     Gets the ID of the <see cref="Chat"/> from the <see cref="Update"/>.
    /// </summary>
    /// <param name="update">The <see cref="Update"/>.</param>
    /// <returns>The ID of the <see cref="Chat"/> or 0 if ID cannot be retrieved.</returns>
    public static long GetChatId(this Update update)
    {
        ArgumentNullException.ThrowIfNull(update);

        return update.Message?.Chat.Id ??
               update.CallbackQuery?.Message?.Chat.Id ??
               update.EditedMessage?.Chat.Id ??
               update.ChannelPost?.Chat.Id ??
               update.EditedChannelPost?.Chat.Id ??
               update.MessageReaction?.Chat.Id ??
               update.MessageReactionCount?.Chat.Id ??
               update.ChatBoost?.Chat.Id ??
               update.RemovedChatBoost?.Chat.Id ??
               update.ChatJoinRequest?.Chat.Id ??
               update.MyChatMember?.Chat.Id ??
               update.ChatMember?.Chat.Id ??
               0;
    }

    /// <summary>
    ///     Gets the type of the <see cref="Chat"/> from the <see cref="Update"/>.
    /// </summary>
    /// <param name="update">The <see cref="Update"/>.</param>
    /// <returns>The type of the <see cref="Chat"/> or <see cref="ChatType.Private"/> by default if type cannot be retrieved.</returns>
    public static ChatType GetChatType(this Update update)
    {
        ArgumentNullException.ThrowIfNull(update);

        return update.Message?.Chat.Type ??
               update.CallbackQuery?.Message?.Chat.Type ??
               update.EditedMessage?.Chat.Type ??
               update.ChannelPost?.Chat.Type ??
               update.EditedChannelPost?.Chat.Type ??
               update.MessageReaction?.Chat.Type ??
               update.MessageReactionCount?.Chat.Type ??
               update.ChatBoost?.Chat.Type ??
               update.RemovedChatBoost?.Chat.Type ??
               update.ChatJoinRequest?.Chat.Type ??
               update.MyChatMember?.Chat.Type ??
               update.ChatMember?.Chat.Type ??
               ChatType.Private;
    }

    /// <summary>
    ///     Determines if the <see cref="Chat"/> is a forum from the <see cref="Update"/>.
    /// </summary>
    /// <param name="update">The <see cref="Update"/>.</param>
    /// <returns>True if the <see cref="Chat"/> is a forum, otherwise false.</returns>
    public static bool IsChatForum(this Update update)
    {
        ArgumentNullException.ThrowIfNull(update);

        return update.Message?.Chat.IsForum ??
               update.CallbackQuery?.Message?.Chat.IsForum ??
               update.EditedMessage?.Chat.IsForum ??
               update.ChannelPost?.Chat.IsForum ??
               update.EditedChannelPost?.Chat.IsForum ??
               update.MessageReaction?.Chat.IsForum ??
               update.MessageReactionCount?.Chat.IsForum ??
               update.ChatBoost?.Chat.IsForum ??
               update.RemovedChatBoost?.Chat.IsForum ??
               update.ChatJoinRequest?.Chat.IsForum ??
               update.MyChatMember?.Chat.IsForum ??
               update.ChatMember?.Chat.IsForum ??
               false;
    }

    /// <summary>
    ///     Gets the message thread ID from the <see cref="Update"/>.
    /// </summary>
    /// <param name="update">The <see cref="Update"/>.</param>
    /// <returns>The message thread ID or 0 if ID cannot be retrieved.</returns>
    public static long GetMessageThreadId(this Update update)
    {
        ArgumentNullException.ThrowIfNull(update);

        return update.Message?.MessageThreadId ??
               update.CallbackQuery?.Message?.MessageThreadId ??
               update.EditedMessage?.MessageThreadId ??
               0;
    }
}
