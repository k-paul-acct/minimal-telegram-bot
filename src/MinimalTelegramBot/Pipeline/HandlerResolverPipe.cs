namespace MinimalTelegramBot.Pipeline;

internal sealed class HandlerResolverPipe : IPipe
{
    public async Task InvokeAsync(BotRequestContext ctx, Func<BotRequestContext, Task> next)
    {
        var handlerBuilder = ctx.Services.GetRequiredService<IHandlerBuilder>();
        var filterContext = new BotRequestFilterContext(ctx);
        var handler = await handlerBuilder.TryResolveHandler(filterContext);

        if (handler is not null)
        {
            ctx.UpdateHandlingStarted = true;
            await handler.Handle(ctx);
        }

        await next(ctx);
    }
}