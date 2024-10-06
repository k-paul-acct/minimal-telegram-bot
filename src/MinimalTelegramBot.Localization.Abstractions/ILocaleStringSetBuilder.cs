namespace MinimalTelegramBot.Localization.Abstractions;

/// <summary>
///     Interface for building the <see cref="LocaleStringSet"/>.
/// </summary>
public interface ILocaleStringSetBuilder
{
    /// <summary>
    ///     Gets the locale associated with the <see cref="LocaleStringSet"/>.
    /// </summary>
    Locale Locale { get; }

    /// <summary>
    ///     Builds and returns a <see cref="LocaleStringSet"/>.
    /// </summary>
    /// <returns>A <see cref="LocaleStringSet"/> representing the built locale string set.</returns>
    LocaleStringSet Build();

    /// <summary>
    ///     Enriches the current <see cref="LocaleStringSet"/> with additional strings.
    /// </summary>
    /// <param name="stringSet">A dictionary containing the strings to enrich the set with.</param>
    /// <returns>The current instance of <see cref="ILocaleStringSetBuilder"/>.</returns>
    ILocaleStringSetBuilder Enrich(IReadOnlyDictionary<string, string> stringSet);
}
