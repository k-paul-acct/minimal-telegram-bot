namespace MinimalTelegramBot.Handling.Filters;

public sealed class BotRequestDelegateFactoryOptions
{
    public required IServiceProvider Services { get; init; }
    public required HandlerBuilder HandlerBuilder { get; init; }
}
