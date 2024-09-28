namespace MinimalTelegramBot.Localization;

internal sealed class InMemoryLocaleStringSetRepository : ILocaleStringSetRepository
{
    private readonly Dictionary<string, IReadOnlyDictionary<string, string>> _translates = new();

    public string GetString(string key, Locale locale)
    {
        if (!_translates.TryGetValue(locale.ToString(), out var stringSet) || !stringSet.TryGetValue(key, out var result))
        {
            throw new KeyNotFoundException($"No string with key {key} found for locale {locale}");
        }

        return result;
    }

    public void AddLocaleStringSet(LocaleStringSet localeStringSet)
    {
        _translates[localeStringSet.Locale.ToString()] = localeStringSet.Values;
    }
}
