using Microsoft.Extensions.Logging;

namespace MinimalTelegramBot.Logging;

internal static partial class InfrastructureLog
{
    private const string CategoryName = "MinimalTelegramBot.Runner";

    public static ILogger CreateLogger(ILoggerFactory loggerFactory)
    {
        return loggerFactory.CreateLogger(CategoryName);
    }

    [LoggerMessage(0, LogLevel.Information, "Getting updates via {gettingUpdatesVia} started for bot @{botUsername} ({botFullName}) with ID = {botId}")]
    public static partial void GettingUpdateStarted(ILogger logger, string gettingUpdatesVia, string botUsername, string botFullName, long botId);

    [LoggerMessage(1, LogLevel.Error, "An unhandled exception was thrown by the application")]
    public static partial void ApplicationError(ILogger logger, Exception ex);

    [LoggerMessage(2, LogLevel.Error, "An unhandled exception was thrown by the application while polling")]
    public static partial void PollingError(ILogger logger, Exception ex);
}
