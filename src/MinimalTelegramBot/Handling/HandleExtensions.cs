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

    public static HandlerBuilder HandleMessage(this IHandlerDispatcher handlerDispatcher)
    {
        throw new NotImplementedException();
    }

    public static HandlerBuilder HandleCallbackDate(this IHandlerDispatcher handlerDispatcher)
    {
        throw new NotImplementedException();
    }
}
