using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MinimalTelegramBot.Pipeline;

internal sealed class UpdateLoggingPipe : IPipe
{
    private readonly ILogger<UpdateLoggingPipe> _logger;

    public UpdateLoggingPipe(ILogger<UpdateLoggingPipe> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(BotRequestContext context, BotRequestDelegate next)
    {
        _logger.LogInformation(0, "Received update with ID = {UpdateId}", context.Update.Id);

        var startingTimestamp = Stopwatch.GetTimestamp();

        await next(context);

        var elapsed = Stopwatch.GetElapsedTime(startingTimestamp);

        if (context.Data.ContainsKey("__UpdateHandlingStarted"))
        {
            _logger.LogInformation(200, "Update with ID = {UpdateId} is handled. Duration: {Milliseconds} ms", context.Update.Id, (long)elapsed.TotalMilliseconds);
        }
        else
        {
            _logger.LogInformation(404, "Update with ID = {UpdateId} is not handled", context.Update.Id);
        }
    }
}
