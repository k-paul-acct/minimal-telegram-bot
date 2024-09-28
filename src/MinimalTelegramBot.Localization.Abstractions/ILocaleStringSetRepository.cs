namespace MinimalTelegramBot.Localization.Abstractions;

/// <summary>
///     Interface for managing multiple <see cref="LocaleStringSet"/>.
/// </summary>
public interface ILocaleStringSetRepository
{
    /// <summary>
    ///     Get string for locale.
    /// </summary>
    /// <param name="key">Key for string.</param>
    /// <param name="locale">Locale.</param>
    /// <returns>Locale specific string.</returns>
    /// <exception cref="KeyNotFoundException">String with key for given locale not found.</exception>
    string GetString(string key, Locale locale);

    /// <summary>
    ///     Adds a new locale-specific string set.
    /// </summary>
    /// <param name="localeStringSet">The locale string set to add.</param>
    void AddLocaleStringSet(LocaleStringSet localeStringSet);
}
