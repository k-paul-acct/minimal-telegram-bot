namespace MinimalTelegramBot.Handling.Filters;

public class BotRequestFilterContext
{
    public IList<object?> FilterArguments { get; internal set; } = [];
    public BotRequestContext BotRequestContext { get; set; }
    public IServiceProvider Services => BotRequestContext.Services;

    public BotRequestFilterContext(BotRequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        BotRequestContext = context;
    }
}