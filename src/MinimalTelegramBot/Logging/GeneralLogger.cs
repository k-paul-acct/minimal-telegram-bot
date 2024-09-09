namespace MinimalTelegramBot.Logging;

internal sealed partial class InfrastructureLogger
{
    public void ApplicationError(Exception ex)
    {
        GeneralLog.ApplicationError(_generalLogger, ex);
    }

    private static partial class GeneralLog
    {
        [LoggerMessage(1, LogLevel.Error, "An unhandled exception was thrown by the application", EventName = "ApplicationError")]
        public static partial void ApplicationError(ILogger logger, Exception ex);
    }
}
