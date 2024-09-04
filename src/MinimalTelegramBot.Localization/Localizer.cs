namespace MinimalTelegramBot.Localization;

/// <inheritdoc />
internal sealed class Localizer : ILocalizer
{
    private readonly ILocaleStringSetRepository _localeStringSetRepository;
    private readonly IBotRequestContextAccessor _contextAccessor;

    public Localizer(ILocaleStringSetRepository localeStringSetRepository, IBotRequestContextAccessor contextAccessor)
    {
        _localeStringSetRepository = localeStringSetRepository;
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    public string GetLocalizedString(string key, params object?[] parameters)
    {
        if (_contextAccessor.BotRequestContext is null)
        {
            throw new Exception($"{nameof(BotRequestContext)} is null");
        }

        var locale = _contextAccessor.BotRequestContext.UserLocale;
        var template = _localeStringSetRepository.GetString(key, locale);
        return parameters.Length == 0 ? template : string.Format(locale.CultureInfo, template, parameters);
    }

    /// <inheritdoc />
    public string GetLocalizedString(Locale locale, string key, params object?[] parameters)
    {
        var template = _localeStringSetRepository.GetString(key, locale);
        return parameters.Length == 0 ? template : string.Format(locale.CultureInfo, template, parameters);
    }
}