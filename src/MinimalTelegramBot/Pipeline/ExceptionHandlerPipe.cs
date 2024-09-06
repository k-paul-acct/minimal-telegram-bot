namespace MinimalTelegramBot.Pipeline;

internal sealed class ExceptionHandlerPipe : IPipe
{
    private readonly ILogger<ExceptionHandlerPipe> _logger;

    public ExceptionHandlerPipe(ILogger<ExceptionHandlerPipe> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(BotRequestContext ctx, Func<BotRequestContext, Task> next)
    {
        try
        {
            await next(ctx);
        }
        catch (Exception e)
        {
            _logger.LogError(1, e, "Unhandled exception: {ExceptionMessage}", e.Message);
        }
    }
}
