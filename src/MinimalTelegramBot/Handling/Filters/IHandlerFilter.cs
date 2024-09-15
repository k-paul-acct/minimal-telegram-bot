namespace MinimalTelegramBot.Handling.Filters;

public interface IHandlerFilter
{
    ValueTask<IResult> InvokeAsync(BotRequestFilterContext context, BotRequestFilterDelegate next);
}
