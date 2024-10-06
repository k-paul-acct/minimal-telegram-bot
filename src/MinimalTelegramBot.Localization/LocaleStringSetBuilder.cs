using System.Collections.Frozen;

namespace MinimalTelegramBot.Localization;

internal sealed class LocaleStringSetBuilder : ILocaleStringSetBuilder
{
    private readonly List<IReadOnlyDictionary<string, string>> _values = [];

    public LocaleStringSetBuilder(Locale locale)
    {
        Locale = locale;
    }

    public Locale Locale { get; }

    public LocaleStringSet Build()
    {
        var combined = _values.SelectMany(x => x).ToFrozenDictionary();
        return new LocaleStringSet
        {
            Locale = Locale,
            Values = combined,
        };
    }

    public ILocaleStringSetBuilder Enrich(IReadOnlyDictionary<string, string> stringSet)
    {
        _values.Add(stringSet);
        return this;
    }
}
