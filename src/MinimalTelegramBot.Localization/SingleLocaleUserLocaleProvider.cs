namespace MinimalTelegramBot.Localization;

internal sealed class SingleLocaleUserLocaleProvider : IUserLocaleProvider
{
    private readonly Locale _locale;

    public SingleLocaleUserLocaleProvider(Locale locale)
    {
        _locale = locale;
    }

    public Task<Locale> GetUserLocaleAsync(long userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_locale);
    }
}
