using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MinimalTelegramBot.Pipeline;

internal sealed partial class UpdateLoggingPipe : IPipe
{
    private readonly ILogger _logger;

    public UpdateLoggingPipe(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("MinimalTelegramBot.UpdateHandling");
    }

    public async Task InvokeAsync(BotRequestContext context, BotRequestDelegate next)
    {
        Log.UpdateReceived(_logger, context.Update.Id);

        var startingTimestamp = Stopwatch.GetTimestamp();

        await next(context);

        var elapsed = Stopwatch.GetElapsedTime(startingTimestamp);

        if (context.Data.ContainsKey("__UpdateHandlingStarted"))
        {
            Log.UpdateHandled(_logger, context.Update.Id, (long)elapsed.TotalMilliseconds);
        }
        else
        {
            Log.UpdateNotHandled(_logger, context.Update.Id);
        }
    }

    private static partial class Log
    {
        [LoggerMessage(0, LogLevel.Debug, "Received update with ID = {updateId}")]
        public static partial void UpdateReceived(ILogger logger, int updateId);

        [LoggerMessage(200, LogLevel.Debug, "Update with ID = {updateId} is handled. Duration: {milliseconds} ms")]
        public static partial void UpdateHandled(ILogger logger, int updateId, long milliseconds);

        [LoggerMessage(404, LogLevel.Debug, "Update with ID = {updateId} is not handled")]
        public static partial void UpdateNotHandled(ILogger logger, int updateId);
    }
}
