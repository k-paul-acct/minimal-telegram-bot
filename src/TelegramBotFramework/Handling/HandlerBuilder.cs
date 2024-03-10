using IResult = TelegramBotFramework.Results.IResult;

namespace TelegramBotFramework.Handling;

public class HandlerBuilder : IHandlerBuilder
{
    private static readonly object LockObj = new();
    private readonly List<Handler> _handlers = [];
    
    public Handler Handle(HandlerDelegate handlerDelegate)
    {
        lock (LockObj)
        {
            var handler = new Handler(handlerDelegate);
            _handlers.Add(handler);
            return handler;
        }
    }

    public Handler Handle(Delegate handlerDelegate)
    {
        lock (LockObj)
        {
            var handler = new Handler(handlerDelegate);
            _handlers.Add(handler);
            return handler;
        }
    }

    public Handler Handle(Func<BotRequestContext, Task> func)
    {
        return Handle((ctx, _) => func(ctx));
    }

    public Handler? TryResolveHandler(BotRequestContext ctx)
    {
        lock (LockObj)
        {
            return _handlers.FirstOrDefault(x => x.CanHandle(ctx));
        }
    }
}

internal static class AwaitableInfo
{
    public static bool IsTypeAwaitable(Type type, out Type? taskType, out Type? genericType)
    {
        taskType = null;
        genericType = null;
        if (type == typeof(Task))
        {
            taskType = typeof(Task);
            return true;
        }
        if (type == typeof(ValueTask))
        {
            taskType = typeof(ValueTask);
            return true;
        }
        if (!type.IsGenericType) return false;
        var genericDefinition = type.GetGenericTypeDefinition();
        if (genericDefinition == typeof(Task<>))
        {
            taskType = typeof(Task);
            genericType = type.GenericTypeArguments[0];
            return true;
        }
        if (genericDefinition != typeof(ValueTask<>)) return false;
        taskType = typeof(ValueTask);
        genericType = type.GenericTypeArguments[0];
        return true;
    }
}

internal static class RequestDelegateHelper
{
    public static Func<BotRequestContext, Task<IResult>> VoidDelegateWrapper(Action<BotRequestContext> delegateInvocation)
    {
        return context =>
        {
            delegateInvocation(context);
            return Task.FromResult(Results.Results.Empty);
        };
    }

    public static Func<BotRequestContext, Task<IResult>> ResultDelegateWrapper(
        Func<BotRequestContext, IResult> delegateInvocation)
    {
        return context => Task.FromResult(delegateInvocation(context));
    }

    public static Func<BotRequestContext, Task<IResult>> GenericDelegateWrapper<T>(
        Func<BotRequestContext, T> delegateInvocation, Func<T, IResult> resultHandler)
    {
        return context =>
        {
            var invocationResult = delegateInvocation(context);
            return Task.FromResult(resultHandler(invocationResult));
        };
    }

    public static Func<BotRequestContext, Task<IResult>> TaskDelegateWrapper(
        Func<BotRequestContext, Task> delegateInvocation)
    {
        return async context =>
        {
            await delegateInvocation(context);
            return Results.Results.Empty;
        };
    }

    public static Func<BotRequestContext, Task<IResult>> GenericTaskDelegateWrapper<T>(
        Func<BotRequestContext, Task<T>> delegateInvocation, Func<T, IResult> resultHandler)
    {
        return async context =>
        {
            var invocationResult = await delegateInvocation(context);
            return resultHandler(invocationResult);
        };
    }
    
    public static Func<BotRequestContext, Task<IResult>> ValueTaskDelegateWrapper(Func<BotRequestContext,
        ValueTask> delegateInvocation)
    {
        return async context =>
        {
            await delegateInvocation(context);
            return Results.Results.Empty;
        };
    }

    public static Func<BotRequestContext, Task<IResult>> ResultValueTaskDelegateWrapper(Func<BotRequestContext,
        ValueTask<IResult>> delegateInvocation)
    {
        return async context => await delegateInvocation(context);
    }

    public static Func<BotRequestContext, Task<IResult>> GenericValueTaskDelegateWrapper<T>(Func<BotRequestContext,
        ValueTask<T>> delegateInvocation, Func<T, IResult> resultHandler)
    {
        return async context =>
        {
            var invocationResult = await delegateInvocation(context);
            return resultHandler(invocationResult);
        };
    }
}