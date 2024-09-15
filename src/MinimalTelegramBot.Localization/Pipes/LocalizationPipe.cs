using MinimalTelegramBot.Pipeline;

namespace MinimalTelegramBot.Localization.Pipes;

internal sealed class LocalizationPipe : IPipe
{
    private readonly IUserLocaleProvider _localeProvider;

    public LocalizationPipe(IUserLocaleProvider localeProvider)
    {
        _localeProvider = localeProvider;
    }

    public async Task InvokeAsync(BotRequestContext ctx, BotRequestDelegate next)
    {
        var locale = await _localeProvider.GetUserLocaleAsync(ctx.ChatId);
        ctx.UserLocale = locale;
        await next(ctx);
    }
}
