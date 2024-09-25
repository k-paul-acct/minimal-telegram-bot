using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling;

public static class HandleExtensions
{
    public static HandlerGroupBuilder HandleGroup(this IHandlerDispatcher handlerDispatcher)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        var groupBuilder = new HandlerGroupBuilder(handlerDispatcher);
        return groupBuilder;
    }

    public static HandlerBuilder Handle(this IHandlerDispatcher handlerDispatcher, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = new HandlerBuilder(handlerDispatcher, handler, null);
        return builder;
    }

    public static HandlerBuilder HandleCommand(this IHandlerDispatcher handlerDispatcher, string command, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterCommand(command);
        return builder;
    }

    public static HandlerBuilder HandleMessageText(this IHandlerDispatcher handlerDispatcher, string messageText, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(messageText);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterMessageText(messageText);
        return builder;
    }

    public static HandlerBuilder HandleCallbackData(this IHandlerDispatcher handlerDispatcher, string callbackData, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(callbackData);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterCallbackData(callbackData);
        return builder;
    }

    public static HandlerBuilder HandleCallbackDataPrefix(this IHandlerDispatcher handlerDispatcher, string callbackDataPrefix, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(callbackDataPrefix);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterCallbackData(x => x.StartsWith(callbackDataPrefix, StringComparison.Ordinal));
        return builder;
    }

    public static HandlerBuilder HandleUpdateType(this IHandlerDispatcher handlerDispatcher, UpdateType updateType, Delegate handler)
    {
        ArgumentNullException.ThrowIfNull(handlerDispatcher);
        ArgumentNullException.ThrowIfNull(handler);

        var builder = handlerDispatcher.Handle(handler);
        builder.FilterUpdateType(updateType);
        return builder;
    }

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
