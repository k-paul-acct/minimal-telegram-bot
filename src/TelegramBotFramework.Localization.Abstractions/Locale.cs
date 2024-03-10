using System.Globalization;

namespace TelegramBotFramework.Localization.Abstractions;

public record Locale
{
    private Locale(string languageCode)
    {
        LanguageCode = languageCode;
    }

    public string LanguageCode { get; }

    private static Locale Russian { get; } = new("ru");
    private static Locale English { get; } = new("en");
    private static Locale Chinese { get; } = new("zh");
    private static Locale German { get; } = new("de");
    private static Locale Kazakh { get; } = new("kk");
    private static Locale Spain { get; } = new("es");

    public static Locale Default { get; set; } = Russian;

    public IFormatProvider StringFormatProvider => new CultureInfo(LanguageCode);

    public static Locale FromLanguageCode(string languageCode)
    {
        return languageCode switch
        {
            "ru" => Russian,
            "es" => Spain,
            "kk" => Kazakh,
            "de" => German,
            "zh" => Chinese,
            "en" => English,
            _ => Default,
        };
    }

    public override string ToString()
    {
        return LanguageCode;
    }
}