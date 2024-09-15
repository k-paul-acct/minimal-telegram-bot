using Microsoft.Extensions.Logging;

namespace MinimalTelegramBot.Logging;

internal sealed partial class InfrastructureLogger
{
    public void ApplicationError(Exception ex)
    {
        GeneralLog.ApplicationError(_generalLogger, ex);
    }

    public void PollingError(Exception ex)
    {
        GeneralLog.PollingError(_generalLogger, ex);
    }

    public void GettingUpdateStarted(bool isWebhook, string botUsername, string botFullName, long botId)
    {
        var gettingUpdatesVia = isWebhook ? "Webhook" : "Polling";
        GeneralLog.GettingUpdateStarted(_generalLogger, gettingUpdatesVia, botUsername, botFullName, botId);
    }

    private static partial class GeneralLog
    {
        [LoggerMessage(1, LogLevel.Error, "An unhandled exception was thrown by the application", EventName = "ApplicationError")]
        public static partial void ApplicationError(ILogger logger, Exception ex);

        [LoggerMessage(2, LogLevel.Error, "An unhandled exception was thrown by the application while polling", EventName = nameof(PollingError))]
        public static partial void PollingError(ILogger logger, Exception ex);

        [LoggerMessage(0, LogLevel.Information, "Getting updates via {gettingUpdatesVia} started for bot @{botUsername} ({botFullName}) with ID = {botId}", EventName = nameof(GettingUpdateStarted))]
        public static partial void GettingUpdateStarted(ILogger logger, string gettingUpdatesVia, string botUsername, string botFullName, long botId);
    }
}
