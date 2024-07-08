using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.Localization.Abstractions.Providers;

namespace MinimalTelegramBot.Localization;

/// <inheritdoc />
public class UserLocaleService : IUserLocaleService
{
    private readonly IUserLocaleProvider _provider;
    private readonly IUserLocaleRepository _repository;

    public UserLocaleService(IUserLocaleRepository repository, IUserLocaleProvider provider)
    {
        _repository = repository;
        _provider = provider;
    }

    /// <inheritdoc />
    public async Task<Locale> UpdateWithProviderAsync(long userId)
    {
        var locale = await _provider.GetUserLocaleAsync(userId);
        _repository.SetUserLocale(userId, locale);
        return locale;
    }

    /// <inheritdoc />
    public async ValueTask<Locale> GetFromRepositoryOrUpdateWithProviderAsync(long userId)
    {
        return _repository.GetUserLocale(userId) ?? await UpdateWithProviderAsync(userId);
    }

    /// <inheritdoc />
    public void Update(long userId, Locale locale)
    {
        _repository.SetUserLocale(userId, locale);
    }

    /// <inheritdoc />
    public Locale? GetFromRepository(long userId)
    {
        return _repository.GetUserLocale(userId);
    }
}