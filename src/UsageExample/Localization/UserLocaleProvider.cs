using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.Localization.Abstractions.Providers;

namespace UsageExample.Localization;

public class UserLocaleProvider : IUserLocaleProvider
{
    public Task<Locale> GetUserLocaleAsync(long userId)
    {
        var locale = new Locale("ru");
        return Task.FromResult(locale);
    }
}