using TelegramBotFramework.Abstractions;
using TelegramBotFramework.Localization.Abstractions;

namespace TelegramBotFramework.Localization;

/// <inheritdoc />
public class Localizer<TUserId> : ILocalizer
{
    private readonly IUserLocaleService<TUserId> _localeService;
    private readonly ILocaleStringSetRepository _localeStringSetRepository;
    private readonly IUserIdProvider<TUserId> _userIdProvider;

    public Localizer(IUserIdProvider<TUserId> userIdProvider, ILocaleStringSetRepository localeStringSetRepository,
        IUserLocaleService<TUserId> localeService)
    {
        _userIdProvider = userIdProvider;
        _localeStringSetRepository = localeStringSetRepository;
        _localeService = localeService;
    }

    /// <inheritdoc />
    public string GetLocalizedString(string key, params object?[] parameters)
    {
        var userId = _userIdProvider.GetUserId();
        var locale = _localeService.GetFromRepository(userId) ?? Locale.Default;
        var template = _localeStringSetRepository.GetString(key, locale.LanguageCode);
        return parameters.Length == 0 ? template : string.Format(locale.StringFormatProvider, template, parameters);
    }

    /// <inheritdoc />
    public string GetLocalizedString(Locale locale, string key, params object?[] parameters)
    {
        var template = _localeStringSetRepository.GetString(key, locale.LanguageCode);
        return parameters.Length == 0 ? template : string.Format(locale.StringFormatProvider, template, parameters);
    }
}