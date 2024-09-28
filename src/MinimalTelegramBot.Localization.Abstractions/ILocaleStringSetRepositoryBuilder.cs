namespace MinimalTelegramBot.Localization.Abstractions;

/// <summary>
///     Interface for building a locale string set repository.
/// </summary>
public interface ILocaleStringSetRepositoryBuilder
{
    /// <summary>
    ///     Adds a locale to the repository.
    /// </summary>
    /// <param name="locale">The locale to add.</param>
    /// <returns>Returns an instance of <see cref="ILocaleStringSetBuilder"/>.</returns>
    ILocaleStringSetBuilder AddLocale(Locale locale);

    /// <summary>
    ///     Builds the locale string set repository.
    /// </summary>
    /// <returns>Returns an instance of <see cref="ILocaleStringSetRepository"/>.</returns>
    ILocaleStringSetRepository Build();
}
