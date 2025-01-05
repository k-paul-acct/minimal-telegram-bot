using System.Collections.Frozen;

namespace MinimalTelegramBot.Localization;

internal sealed class InMemoryLocaleStringSetRepository : ILocaleStringSetRepository
{
    private readonly FrozenDictionary<string, IReadOnlyDictionary<string, string>> _translates;

    public InMemoryLocaleStringSetRepository(IEnumerable<LocaleStringSet> localeStringSets)
    {
        _translates = localeStringSets
            .Select(x => new KeyValuePair<string, IReadOnlyDictionary<string, string>>(x.Locale.ToString(), x.Values))
            .ToFrozenDictionary();
    }

    public string GetString(string key, Locale locale)
    {
        if (!_translates.TryGetValue(locale.ToString(), out var stringSet) || !stringSet.TryGetValue(key, out var result))
        {
            throw new KeyNotFoundException($"No string with key {key} found for locale {locale}");
        }

        return result;
    }
}
