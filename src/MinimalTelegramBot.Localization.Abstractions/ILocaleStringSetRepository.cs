namespace MinimalTelegramBot.Localization.Abstractions;

public interface ILocaleStringSetRepository
{
    /// <summary>
    ///     Get string for locale.
    /// </summary>
    /// <param name="key">Key for string.</param>
    /// <param name="locale">Locale.</param>
    /// <returns>Locale specific string.</returns>
    /// <exception cref="Exception">String with key for given locale not found.</exception>
    string GetString(string key, string locale);

    void AddLocaleStringSet(LocaleStringSet localeStringSet);
}