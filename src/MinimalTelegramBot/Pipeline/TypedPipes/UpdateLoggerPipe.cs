using System.Diagnostics;

namespace MinimalTelegramBot.Pipeline.TypedPipes;

internal sealed class UpdateLoggerPipe : IPipe
{
    private readonly ILogger<UpdateLoggerPipe> _logger;

    public UpdateLoggerPipe(ILogger<UpdateLoggerPipe> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(BotRequestContext ctx, BotRequestDelegate next)
    {
        _logger.LogInformation(1, "Received update with ID = {UpdateId}", ctx.Update.Id);

        var stopWatch = Stopwatch.StartNew();

        await next(ctx);

        stopWatch.Stop();

        if (!ctx.UpdateHandlingStarted)
        {
            _logger.LogInformation(2, "Update with ID = {UpdateId} is not handled", ctx.Update.Id);
        }
        else
        {
            _logger.LogInformation(3, "Update with ID = {UpdateId} is handled. Duration {Milliseconds} ms", ctx.Update.Id, stopWatch.ElapsedMilliseconds);
        }
    }
}
