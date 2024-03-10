using System.Collections.Concurrent;
using TelegramBotFramework.Localization.Abstractions;

namespace TelegramBotFramework.Localization;

/// <inheritdoc />
public class InMemoryUserLocaleRepository<TUserId> : IUserLocaleRepository<TUserId> where TUserId : notnull
{
    private readonly ConcurrentDictionary<TUserId, Locale> _locales = new();

    /// <inheritdoc />
    public Locale? GetUserLocale(TUserId userId)
    {
        _ = _locales.TryGetValue(userId, out var locale);
        return locale;
    }

    /// <inheritdoc />
    public void SetUserLocale(TUserId userId, Locale locale)
    {
        _locales.AddOrUpdate(userId, locale, (_, _) => locale);
    }
}