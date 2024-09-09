using MinimalTelegramBot.Logging;

namespace MinimalTelegramBot;

internal sealed class UpdateHandlingInvocationContext : IDisposable, IAsyncDisposable
{
    public required BotRequestContext BotRequestContext { get; init; }
    public required AsyncServiceScope ServiceScope { get; init; }
    public required BotRequestDelegate Pipeline { get; init; }
    public required InfrastructureLogger Logger { get; init; }

    public ValueTask DisposeAsync()
    {
        return ServiceScope.DisposeAsync();
    }

    public void Dispose()
    {
        ServiceScope.Dispose();
    }

    public async Task Invoke()
    {
        try
        {
            await Pipeline(BotRequestContext);
        }
        catch (Exception ex)
        {
            Logger.ApplicationError(ex);
        }
        finally
        {
            BotRequestContext.Client.FlushHttpContent();
            await DisposeAsync();
        }
    }
}
