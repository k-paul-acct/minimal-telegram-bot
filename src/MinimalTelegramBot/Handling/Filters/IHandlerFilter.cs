namespace MinimalTelegramBot.Handling.Filters;

public interface IHandlerFilter
{
    ValueTask<bool> Filter(BotRequestFilterContext context);
}
