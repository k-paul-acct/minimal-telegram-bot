using System.Collections.Concurrent;
using MinimalTelegramBot.Localization.Abstractions;

namespace MinimalTelegramBot.Localization;

/// <inheritdoc />
public class InMemoryUserLocaleRepository : IUserLocaleRepository
{
    private readonly ConcurrentDictionary<long, Locale> _locales = new();

    /// <inheritdoc />
    public Locale? GetUserLocale(long userId)
    {
        _ = _locales.TryGetValue(userId, out var locale);
        return locale;
    }

    /// <inheritdoc />
    public void SetUserLocale(long userId, Locale locale)
    {
        _locales.AddOrUpdate(userId, locale, (_, _) => locale);
    }
}