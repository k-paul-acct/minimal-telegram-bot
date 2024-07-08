namespace MinimalTelegramBot.Localization.Abstractions;

public class LocaleStringSet
{
    public required Locale Locale { get; init; }
    public required IReadOnlyDictionary<string, string> Values { get; init; }
}