using System.Globalization;

namespace MinimalTelegramBot.Localization.Abstractions;

/// <summary>
///     Represents a locale with language and optional region code.
/// </summary>
public sealed class Locale : IEquatable<Locale>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Locale"/> with the specified language and region codes.
    /// </summary>
    /// <param name="languageCode">The language code (e.g., "en" for English).</param>
    /// <param name="regionCode">The optional region code (e.g., "US" for United States).</param>
    public Locale(string languageCode, string? regionCode)
    {
        ArgumentNullException.ThrowIfNull(languageCode);

        LanguageCode = languageCode.ToLowerInvariant();
        RegionCode = regionCode?.ToUpperInvariant();
        FullCode = RegionCode is null ? LanguageCode : $"{LanguageCode}-{RegionCode}";
        CultureInfo = CultureInfo.GetCultureInfo(FullCode);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Locale"/> with the specified full locale code.
    /// </summary>
    /// <param name="fullCode">The full locale code (e.g., "en-US" for English (United States)).</param>
    public Locale(string fullCode)
    {
        ArgumentNullException.ThrowIfNull(fullCode);

        var span = fullCode.AsSpan();
        var hyphenIndex = span.IndexOf('-');

        if (hyphenIndex == -1)
        {
            LanguageCode = fullCode.ToLowerInvariant();
        }
        else
        {
            LanguageCode = new string(span[..hyphenIndex]).ToLowerInvariant();
            RegionCode = new string(span[(hyphenIndex + 1)..]).ToUpperInvariant();
        }

        FullCode = RegionCode is null ? LanguageCode : $"{LanguageCode}-{RegionCode}";
        CultureInfo = CultureInfo.GetCultureInfo(FullCode);
    }

    /// <summary>
    ///     Gets the language code (e.g., "en" for English).
    /// </summary>
    public string LanguageCode { get; }

    /// <summary>
    ///     Gets the optional region code (e.g., "US" for United States).
    /// </summary>
    public string? RegionCode { get; }

    /// <summary>
    ///     Gets the full locale code (e.g., "en-US" for English (United States)).
    /// </summary>
    public string FullCode { get; }

    /// <summary>
    ///     Gets the <see cref="CultureInfo"/> object for the locale.
    /// </summary>
    public CultureInfo CultureInfo { get; }

    /// <summary>
    ///     Gets or sets the default locale. Default locale is "en".
    /// </summary>
    public static Locale Default { get; set; } = new("en", null);

    /// <summary>
    ///     Determines whether the specified <see cref="Locale"/> is equal to the current <see cref="Locale"/>.
    /// </summary>
    /// <param name="other">The <see cref="Locale"/> to compare with the current <see cref="Locale"/>.</param>
    /// <returns>
    ///     <c>true</c> if the specified <see cref="Locale"/> is equal to the current <see cref="Locale"/>; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(Locale? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(FullCode, other.FullCode, StringComparison.Ordinal);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return FullCode;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return StringComparer.Ordinal.GetHashCode(FullCode);
    }

    /// <summary>
    ///     Determines whether the specified object is equal to the current <see cref="Locale"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Locale"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="Locale"/>; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        return obj is Locale other && Equals(other);
    }

    /// <summary>
    ///     Determines whether two <see cref="Locale"/> instances are equal.
    /// </summary>
    /// <param name="a">The first <see cref="Locale"/> to compare.</param>
    /// <param name="b">The second <see cref="Locale"/> to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Locale"/> instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Locale? a, Locale? b)
    {
        if (a is not null)
        {
            return a.Equals(b);
        }

        return b is null;
    }

    /// <summary>
    ///     Determines whether two <see cref="Locale"/> instances are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="Locale"/> to compare.</param>
    /// <param name="b">The second <see cref="Locale"/> to compare.</param>
    /// <returns><c>true</c> if the specified <see cref="Locale"/> instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Locale? a, Locale? b)
    {
        return !(a == b);
    }
}
