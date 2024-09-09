using MinimalTelegramBot.Handling.Filters;

namespace MinimalTelegramBot.Localization.Filters;

internal sealed class LocalizedTextFilter : IHandlerFilter
{
    private readonly ILocalizer _localizer;

    public LocalizedTextFilter(ILocalizer localizer)
    {
        _localizer = localizer;
    }

    public ValueTask<bool> Filter(BotRequestFilterContext context, Func<BotRequestFilterContext, ValueTask<bool>> next)
    {
        var key = (string)context.FilterArguments[0]!;
        var pass = context.BotRequestContext.MessageText is not null && _localizer[key] == context.BotRequestContext.MessageText;
        return pass ? next(context) : ValueTask.FromResult(false);
    }
}
