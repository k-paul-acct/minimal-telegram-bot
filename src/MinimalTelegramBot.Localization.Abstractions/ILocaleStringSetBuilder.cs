namespace MinimalTelegramBot.Localization.Abstractions;

public interface ILocaleStringSetBuilder
{
    Locale Locale { get; }
    LocaleStringSet Build();
    ILocaleStringSetBuilder Enrich(IReadOnlyDictionary<string, string> stringSet);
}
