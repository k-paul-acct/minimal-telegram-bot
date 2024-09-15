using MinimalTelegramBot.Handling.Filters;

namespace MinimalTelegramBot.Localization.Filters;

internal sealed class LocalizedTextFilter : IHandlerFilter
{
    private readonly ILocalizer _localizer;

    public LocalizedTextFilter(ILocalizer localizer)
    {
        _localizer = localizer;
    }

    public ValueTask<IResult> InvokeAsync(BotRequestFilterContext context, BotRequestFilterDelegate next)
    {
        if (context.BotRequestContext.MessageText is null)
        {
            return new ValueTask<IResult>(Results.Results.Empty);
        }

        var key = (string)context.Arguments[^1]!;
        return _localizer[key] == context.BotRequestContext.MessageText ? next(context) : new ValueTask<IResult>(Results.Results.Empty);
    }
}
