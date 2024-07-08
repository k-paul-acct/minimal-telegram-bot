namespace TelegramBotFramework.Localization.Abstractions;

public abstract class ApplicationLocales
{
    private readonly Dictionary<string, Locale> _locales = new();
    
    public Locale GetByFullCode(string fullCode)
    {
        return _locales[fullCode];
    }

    public void Add(string fullCode)
    {
        
    }
}