namespace MinimalTelegramBot.Localization.Abstractions;

/// <summary>
///     Interface for localizing strings.
/// </summary>
public interface ILocalizer
{
    /// <summary>
    ///     Indexer to get the localized string for the specified key.
    /// </summary>
    /// <param name="key">The key of the string to localize.</param>
    /// <param name="parameters">Optional parameters to format the localized string.</param>
    /// <exception cref="KeyNotFoundException">String with key for current user locale not found.</exception>
    string this[string key, params object?[] parameters] => GetLocalizedString(key, parameters);

    /// <summary>
    ///     Indexer to get the localized string for the specified locale and key.
    /// </summary>
    /// <param name="locale">The locale for which to get the localized string.</param>
    /// <param name="key">The key of the string to localize.</param>
    /// <param name="parameters">Optional parameters to format the localized string.</param>
    /// <exception cref="KeyNotFoundException">String with key for the given locale not found.</exception>
    string this[Locale locale, string key, params object?[] parameters] => GetLocalizedString(locale, key, parameters);

    /// <summary>
    ///     Gets the localized string for the specified key.
    /// </summary>
    /// <param name="key">The key of the string to localize.</param>
    /// <param name="parameters">Optional parameters to format the localized string.</param>
    /// <returns>The localized string.</returns>
    /// <exception cref="KeyNotFoundException">String with key for current user locale not found.</exception>
    string GetLocalizedString(string key, params object?[] parameters);

    /// <summary>
    ///     Gets the localized string for the specified locale and key.
    /// </summary>
    /// <param name="locale">The locale for which to get the localized string.</param>
    /// <param name="key">The key of the string to localize.</param>
    /// <param name="parameters">Optional parameters to format the localized string.</param>
    /// <returns>The localized string.</returns>
    /// <exception cref="KeyNotFoundException">String with key for the given locale not found.</exception>
    string GetLocalizedString(Locale locale, string key, params object?[] parameters);
}
