namespace MinimalTelegramBot.Localization;

internal sealed class LocaleStringSetRepositoryBuilder : ILocaleStringSetRepositoryBuilder
{
    private readonly List<LocaleStringSetBuilder> _setBuilders = [];

    public ILocaleStringSetBuilder AddLocale(Locale locale)
    {
        var setBuilder = new LocaleStringSetBuilder(locale);
        _setBuilders.Add(setBuilder);
        return setBuilder;
    }

    public ILocaleStringSetRepository Build()
    {
        var sets = _setBuilders.Select(x => x.Build());
        return new InMemoryLocaleStringSetRepository(sets);
    }
}
