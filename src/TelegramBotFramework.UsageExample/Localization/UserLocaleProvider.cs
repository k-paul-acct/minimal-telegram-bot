using TelegramBotFramework.Localization.Abstractions;
using TelegramBotFramework.Localization.Abstractions.Providers;

namespace TelegramBotFramework.UsageExample.Localization;

public class UserLocaleProvider : IUserLocaleProvider
{
    public Task<Locale> GetUserLocaleAsync(long userId)
    {
        var locale = new Locale("ru");
        return Task.FromResult(locale);
    }
}