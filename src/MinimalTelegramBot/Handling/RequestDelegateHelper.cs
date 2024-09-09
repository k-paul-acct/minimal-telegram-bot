namespace MinimalTelegramBot.Handling;

internal static class RequestDelegateHelper
{
    public static HandlerDelegate VoidDelegateWrapper(Action<BotRequestContext> delegateInvocation)
    {
        return context =>
        {
            delegateInvocation(context);
            return Task.FromResult(Results.Results.Empty);
        };
    }

    public static HandlerDelegate ResultDelegateWrapper(Func<BotRequestContext, IResult> delegateInvocation)
    {
        return context => Task.FromResult(delegateInvocation(context));
    }

    public static HandlerDelegate GenericDelegateWrapper<T>(Func<BotRequestContext, T> delegateInvocation, Func<T, IResult> resultHandler)
    {
        return context =>
        {
            var invocationResult = delegateInvocation(context);
            return Task.FromResult(resultHandler(invocationResult));
        };
    }

    public static HandlerDelegate TaskDelegateWrapper(Func<BotRequestContext, Task> delegateInvocation)
    {
        return async context =>
        {
            await delegateInvocation(context);
            return Results.Results.Empty;
        };
    }

    public static HandlerDelegate GenericTaskDelegateWrapper<T>(Func<BotRequestContext, Task<T>> delegateInvocation, Func<T, IResult> resultHandler)
    {
        return async context =>
        {
            var invocationResult = await delegateInvocation(context);
            return resultHandler(invocationResult);
        };
    }

    public static HandlerDelegate ValueTaskDelegateWrapper(Func<BotRequestContext, ValueTask> delegateInvocation)
    {
        return async context =>
        {
            await delegateInvocation(context);
            return Results.Results.Empty;
        };
    }

    public static HandlerDelegate ResultValueTaskDelegateWrapper(Func<BotRequestContext, ValueTask<IResult>> delegateInvocation)
    {
        return context => delegateInvocation(context).AsTask();
    }

    public static HandlerDelegate GenericValueTaskDelegateWrapper<T>(Func<BotRequestContext, ValueTask<T>> delegateInvocation, Func<T, IResult> resultHandler)
    {
        return async context =>
        {
            var invocationResult = await delegateInvocation(context);
            return resultHandler(invocationResult);
        };
    }
}
