namespace MinimalTelegramBot.Pipeline.TypedPipes;

internal sealed class BotRequestContextAccessorPipe : IPipe
{
    private readonly IBotRequestContextAccessor _contextAccessor;

    public BotRequestContextAccessorPipe(IBotRequestContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Task InvokeAsync(BotRequestContext context, BotRequestDelegate next)
    {
        _contextAccessor.BotRequestContext = context;
        return next(context);
    }
}
