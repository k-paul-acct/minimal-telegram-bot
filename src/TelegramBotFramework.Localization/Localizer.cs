using TelegramBotFramework.Localization.Abstractions;

namespace TelegramBotFramework.Localization;

/// <inheritdoc />
public class Localizer : ILocalizer
{
    private readonly IUserLocaleService _localeService;
    private readonly BotRequestContext? _context;
    private readonly ILocaleStringSetRepository _localeStringSetRepository;

    public Localizer(IBotRequestContextAccessor contextAccessor, ILocaleStringSetRepository localeStringSetRepository,
        IUserLocaleService localeService)
    {
        _context = contextAccessor.BotRequestContext;
        _localeStringSetRepository = localeStringSetRepository;
        _localeService = localeService;
    }

    /// <inheritdoc />
    public string GetLocalizedString(string key, params object?[] parameters)
    {
        if (_context is null)
        {
            throw new Exception($"{nameof(BotRequestContext)} is null");
        }

        var userId = _context.ChatId;
        var locale = _localeService.GetFromRepository(userId) ?? Locale.Default;
        var template = _localeStringSetRepository.GetString(key, locale.LanguageCode);
        return parameters.Length == 0 ? template : string.Format(locale.CultureInfo, template, parameters);
    }

    /// <inheritdoc />
    public string GetLocalizedString(Locale locale, string key, params object?[] parameters)
    {
        var template = _localeStringSetRepository.GetString(key, locale.LanguageCode);
        return parameters.Length == 0 ? template : string.Format(locale.CultureInfo, template, parameters);
    }
}