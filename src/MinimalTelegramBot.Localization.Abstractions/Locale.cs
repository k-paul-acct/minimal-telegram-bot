using System.Globalization;

namespace MinimalTelegramBot.Localization.Abstractions;

public sealed class Locale : IEquatable<Locale>
{
    public Locale(string languageCode, string? regionCode)
    {
        ArgumentNullException.ThrowIfNull(languageCode);

        LanguageCode = languageCode.ToLowerInvariant();
        RegionCode = regionCode?.ToUpperInvariant();
        FullCode = RegionCode is null ? LanguageCode : $"{LanguageCode}-{RegionCode}";
        CultureInfo = CultureInfo.GetCultureInfo(FullCode);
    }

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

    public string LanguageCode { get; }
    public string? RegionCode { get; }
    public string FullCode { get; }
    public CultureInfo CultureInfo { get; }

    public static Locale Default { get; set; } = new("en", null);

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

    public override string ToString()
    {
        return FullCode;
    }

    public override int GetHashCode()
    {
        return StringComparer.Ordinal.GetHashCode(FullCode);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        return obj is Locale other && Equals(other);
    }

    public static bool operator ==(Locale? a, Locale? b)
    {
        if (a is not null)
        {
            return a.Equals(b);
        }

        return b is null;
    }

    public static bool operator !=(Locale? a, Locale? b)
    {
        return !(a == b);
    }
}
