namespace MinimalTelegramBot.Localization.Abstractions;

/// <summary>
///     Interface for building locale string set.
/// </summary>
public interface ILocaleStringSetBuilder
{
    /// <summary>
    ///     Gets the locale associated with the string set.
    /// </summary>
    Locale Locale { get; }

    /// <summary>
    ///     Builds and returns a locale string set.
    /// </summary>
    /// <returns>A <see cref="LocaleStringSet"/> representing the built locale string set.</returns>
    LocaleStringSet Build();

    /// <summary>
    ///     Enriches the current locale string set with additional strings.
    /// </summary>
    /// <param name="stringSet">A dictionary containing the strings to enrich the set with.</param>
    /// <returns>The current instance of <see cref="ILocaleStringSetBuilder"/>.</returns>
    ILocaleStringSetBuilder Enrich(IReadOnlyDictionary<string, string> stringSet);
}
