using MinimalTelegramBot.Pipeline;

namespace MinimalTelegramBot.Localization.Pipes;

internal sealed class LocalizationPipe : IPipe
{
    private readonly IUserLocaleProvider _localeProvider;

    public LocalizationPipe(IUserLocaleProvider localeProvider)
    {
        _localeProvider = localeProvider;
    }

    public async Task InvokeAsync(BotRequestContext context, BotRequestDelegate next)
    {
        var locale = await _localeProvider.GetUserLocaleAsync(context.ChatId);
        context.UserLocale = locale;
        await next(context);
    }
}
