namespace MinimalTelegramBot.Server;

internal abstract class UpdateServerInvocationContext : IDisposable, IAsyncDisposable
{
    protected UpdateServerInvocationContext(AsyncServiceScope serviceScope, BotRequestContext botRequestContext)
    {
        ServiceScope = serviceScope;
        BotRequestContext = botRequestContext;
    }

    private AsyncServiceScope ServiceScope { get; }
    public BotRequestContext BotRequestContext { get; }

    public virtual ValueTask DisposeAsync()
    {
        return ServiceScope.DisposeAsync();
    }

    public virtual void Dispose()
    {
        ServiceScope.Dispose();
    }
}
