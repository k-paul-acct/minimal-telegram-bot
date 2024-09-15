using Microsoft.Extensions.Logging;

namespace MinimalTelegramBot.Logging;

internal sealed partial class InfrastructureLogger : ILogger
{
    private readonly ILogger _generalLogger;

    public InfrastructureLogger(ILoggerFactory loggerFactory)
    {
        _generalLogger = loggerFactory.CreateLogger("MinimalTelegramBot.Infrastructure.Runner");
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _generalLogger.Log(logLevel, eventId, state, exception, formatter);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _generalLogger.IsEnabled(logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _generalLogger.BeginScope(state);
    }
}
