using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling;

/// <summary>
///     HandleExtensions.
/// </summary>
public static class HandleExtensions
{
    /// <summary>
    ///     Creates a new <see cref="HandlerGroupBuilder"/> for a group of handlers to define common conventions such as filters
    ///     for each <see cref="Handler"/> created from that group.
    /// </summary>
    /// <param name="handlerDispatcher">The origin handler dispatcher to use for handler group creation.</param>
    /// <returns>A new instance of <see cref="HandlerGroupBuilder"/>.</returns>
    public static HandlerGroupBuilder HandleGroup(this IHandlerDispatcher handlerDispatcher)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        var groupBuilder = new HandlerGroupBuilder(handlerDispatcher);
        return groupBuilder;
    }

    /// <summary>
    ///     Creates a new <see cref="HandlerBuilder"/> for a generic handler.
    /// </summary>
    /// <param name="handlerDispatcher">The origin handler dispatcher to use for handler creation.</param>
    /// <param name="handler">The delegate representing the handler logic.</param>
    /// <returns>A new instance of <see cref="HandlerBuilder"/>.</returns>
    public static HandlerBuilder Handle(this IHandlerDispatcher handlerDispatcher, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = new HandlerBuilder(handlerDispatcher, handler);
        return builder;
    }

    /// <summary>
    ///     Creates a new <see cref="HandlerBuilder"/> for a bot command handler.
    /// </summary>
    /// <remarks>
    ///     Additional command arguments, such as <c>123</c> in <c>/start 123</c> command, are not counted.
    /// </remarks>
    /// <param name="handlerDispatcher">The origin handler dispatcher to use for handler creation.</param>
    /// <param name="command">The command string to filter the handler, for example <c>/start</c>.</param>
    /// <param name="handler">The delegate representing the handler logic.</param>
    /// <returns>A new instance of <see cref="HandlerBuilder"/>.</returns>
    public static HandlerBuilder HandleCommand(this IHandlerDispatcher handlerDispatcher, string command, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterCommand(command);
        return builder;
    }

    /// <summary>
    ///     Creates a new <see cref="HandlerBuilder"/> for a message text handler.
    /// </summary>
    /// <param name="handlerDispatcher">The origin handler dispatcher to use for handler creation.</param>
    /// <param name="messageText">The message text to filter the handler.</param>
    /// <param name="handler">The delegate representing the handler logic.</param>
    /// <returns>A new instance of <see cref="HandlerBuilder"/>.</returns>
    public static HandlerBuilder HandleMessageText(this IHandlerDispatcher handlerDispatcher, string messageText, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(messageText);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterMessageText(messageText);
        return builder;
    }

    /// <summary>
    ///     Creates a new <see cref="HandlerBuilder"/> for a callback data handler.
    /// </summary>
    /// <param name="handlerDispatcher">The origin handler dispatcher to use for handler creation.</param>
    /// <param name="callbackData">The callback data to filter the handler.</param>
    /// <param name="handler">The delegate representing the handler logic.</param>
    /// <returns>A new instance of <see cref="HandlerBuilder"/>.</returns>
    public static HandlerBuilder HandleCallbackData(this IHandlerDispatcher handlerDispatcher, string callbackData, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(callbackData);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterCallbackData(callbackData);
        return builder;
    }

    /// <summary>
    ///     Creates a new <see cref="HandlerBuilder"/> for a callback data handler with a specific prefix.
    /// </summary>
    /// <param name="handlerDispatcher">The origin handler dispatcher to use for handler creation.</param>
    /// <param name="callbackDataPrefix">The prefix of the callback data to filter the handler.</param>
    /// <param name="handler">The delegate representing the handler logic.</param>
    /// <returns>A new instance of <see cref="HandlerBuilder"/>.</returns>
    public static HandlerBuilder HandleCallbackDataPrefix(this IHandlerDispatcher handlerDispatcher, string callbackDataPrefix, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(callbackDataPrefix);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterCallbackData(x => x.StartsWith(callbackDataPrefix, StringComparison.Ordinal));
        return builder;
    }

    /// <summary>
    ///     Creates a new <see cref="HandlerBuilder"/> for a specific update type handler.
    /// </summary>
    /// <param name="handlerDispatcher">The origin handler dispatcher to use for handler creation.</param>
    /// <param name="updateType">The type of update to filter the handler.</param>
    /// <param name="handler">The delegate representing the handler logic.</param>
    /// <returns>A new instance of <see cref="HandlerBuilder"/>.</returns>
    public static HandlerBuilder HandleUpdateType(this IHandlerDispatcher handlerDispatcher, UpdateType updateType, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterUpdateType(updateType);
        return builder;
    }

    /// <summary>
    ///     Creates a new <see cref="HandlerBuilder"/> for an inline query handler.
    /// </summary>
    /// <param name="handlerDispatcher">The origin handler dispatcher to use for handler creation.</param>
    /// <param name="query">The inline query to filter the handler.</param>
    /// <param name="handler">The delegate representing the handler logic.</param>
    /// <returns>A new instance of <see cref="HandlerBuilder"/>.</returns>
    public static HandlerBuilder HandleInlineQuery(this IHandlerDispatcher handlerDispatcher, string query, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterInlineQuery(query);
        return builder;
    }
}
