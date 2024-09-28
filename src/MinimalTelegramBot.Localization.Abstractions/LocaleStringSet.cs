namespace MinimalTelegramBot.Localization.Abstractions;

/// <summary>
///     Represents a set of localized strings for a specific locale.
/// </summary>
public class LocaleStringSet
{
    /// <summary>
    ///     Gets the locale associated with this set of localized strings.
    /// </summary>
    public required Locale Locale { get; init; }

    /// <summary>
    ///     Gets the dictionary of localized string values.
    /// </summary>
    public required IReadOnlyDictionary<string, string> Values { get; init; }
}
