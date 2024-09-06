namespace MinimalTelegramBot.Localization.Abstractions;

public interface ILocaleStringSetRepositoryBuilder
{
    ILocaleStringSetBuilder AddLocale(Locale locale);
    ILocaleStringSetRepository Build();
}
