using TelegramBotFramework.Localization.Abstractions;

namespace TelegramBotFramework.Localization;

/// <inheritdoc />
public class LocaleStringSetBuilder : ILocaleStringSetBuilder
{
    private readonly List<IDictionary<string, string>> _values = [];

    public LocaleStringSetBuilder(Locale locale)
    {
        Locale = locale;
    }

    /// <inheritdoc />
    public Locale Locale { get; }

    /// <inheritdoc />
    public LocaleStringSet Build()
    {
        var combined = _values.SelectMany(x => x).ToDictionary();
        return new LocaleStringSet
        {
            Locale = Locale,
            Values = combined,
        };
    }

    /// <inheritdoc />
    public ILocaleStringSetBuilder Enrich(IDictionary<string, string> stringSet)
    {
        _values.Add(stringSet);
        return this;
    }
}