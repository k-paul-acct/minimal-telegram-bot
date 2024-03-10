namespace TelegramBotFramework.Localization.Abstractions;

public class LocaleStringSet
{
    public required Locale Locale { get; init; }
    public IReadOnlyDictionary<string, string> Values { get; init; } = new Dictionary<string, string>();
}