using TelegramBotFramework.Localization.Abstractions;
using TelegramBotFramework.Localization.Abstractions.Providers;

namespace TelegramBotFramework.Localization;

/// <inheritdoc />
public class UserLocaleService<TUserId> : IUserLocaleService<TUserId>
{
    private readonly IUserLocaleProvider<TUserId> _provider;
    private readonly IUserLocaleRepository<TUserId> _repository;

    public UserLocaleService(IUserLocaleRepository<TUserId> repository, IUserLocaleProvider<TUserId> provider)
    {
        _repository = repository;
        _provider = provider;
    }

    /// <inheritdoc />
    public async Task<Locale> UpdateWithProviderAsync(TUserId userId)
    {
        var locale = await _provider.GetUserLocaleAsync(userId);
        _repository.SetUserLocale(userId, locale);
        return locale;
    }

    /// <inheritdoc />
    public async ValueTask<Locale> GetFromRepositoryOrUpdateWithProviderAsync(TUserId userId)
    {
        return _repository.GetUserLocale(userId) ?? await UpdateWithProviderAsync(userId);
    }

    /// <inheritdoc />
    public void Update(TUserId userId, Locale locale)
    {
        _repository.SetUserLocale(userId, locale);
    }

    /// <inheritdoc />
    public Locale? GetFromRepository(TUserId userId)
    {
        return _repository.GetUserLocale(userId);
    }
}