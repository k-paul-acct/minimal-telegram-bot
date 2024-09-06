namespace MinimalTelegramBot.Handling.Filters;

public class BotRequestFilterContext
{
    public BotRequestFilterContext(BotRequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        BotRequestContext = context;
    }

    public IList<object?> FilterArguments { get; internal set; } = [];
    public BotRequestContext BotRequestContext { get; set; }
    public IServiceProvider Services => BotRequestContext.Services;
}
