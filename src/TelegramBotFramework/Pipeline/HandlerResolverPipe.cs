using TelegramBotFramework.Handling;

namespace TelegramBotFramework.Pipeline;

public class HandlerResolverPipe : IPipe
{
    public async Task InvokeAsync(BotRequestContext ctx, BotRequestDelegate next)
    {
        var handlerBuilder = ctx.Services.GetRequiredService<IHandlerBuilder>();
        var handler = handlerBuilder.TryResolveHandler(ctx);

        if (handler is not null)
        {
            ctx.UpdateHandlingStarted = true;
            await handler.Handle(ctx);
        }

        await next(ctx);
    }
}