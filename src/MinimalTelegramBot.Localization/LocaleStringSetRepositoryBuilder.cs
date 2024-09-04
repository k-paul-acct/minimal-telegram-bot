namespace MinimalTelegramBot.Localization;

internal sealed class LocaleStringSetRepositoryBuilder : ILocaleStringSetRepositoryBuilder
{
    private readonly InMemoryLocaleStringSetRepository _repository = new();
    private readonly Dictionary<Locale, LocaleStringSetBuilder> _setBuilders = new();

    public ILocaleStringSetBuilder AddLocale(Locale locale)
    {
        var setBuilder = new LocaleStringSetBuilder(locale);
        _setBuilders.Add(locale,setBuilder);
        return setBuilder;
    }

    public ILocaleStringSetRepository Build()
    {
        var sets = _setBuilders.Select(x => x.Value.Build());

        foreach (var set in sets)
        {
            _repository.AddLocaleStringSet(set);
        }

        return _repository;
    }
}