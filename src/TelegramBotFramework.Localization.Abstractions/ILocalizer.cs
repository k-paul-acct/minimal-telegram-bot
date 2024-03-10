namespace TelegramBotFramework.Localization.Abstractions;

public interface ILocalizer
{
    string this[string key, params object?[] parameters] => GetLocalizedString(key, parameters);
    string this[Locale locale, string key, params object?[] parameters] => GetLocalizedString(locale, key, parameters);
    string GetLocalizedString(string key, params object?[] parameters);
    string GetLocalizedString(Locale locale, string key, params object?[] parameters);
}