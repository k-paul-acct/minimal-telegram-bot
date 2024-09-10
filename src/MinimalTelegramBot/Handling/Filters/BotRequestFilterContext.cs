namespace MinimalTelegramBot.Handling.Filters;

public sealed class BotRequestFilterContext
{
    public BotRequestFilterContext(BotRequestContext context)
    {
        BotRequestContext = context;
    }

    public List<object?> Arguments { get; } = [];
    public BotRequestContext BotRequestContext { get; }
    public IServiceProvider Services => BotRequestContext.Services;
}
