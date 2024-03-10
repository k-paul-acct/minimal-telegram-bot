namespace TelegramBotFramework.Localization.Abstractions;

public interface ILocaleStringSetBuilder
{
    Locale Locale { get; }
    LocaleStringSet Build();
    ILocaleStringSetBuilder Enrich(IDictionary<string, string> stringSet);
}