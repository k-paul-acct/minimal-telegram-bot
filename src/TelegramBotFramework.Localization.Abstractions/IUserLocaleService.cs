namespace TelegramBotFramework.Localization.Abstractions;

public interface IUserLocaleService<in TUserId>
{
    Task<Locale> UpdateWithProviderAsync(TUserId userId);
    ValueTask<Locale> GetFromRepositoryOrUpdateWithProviderAsync(TUserId userId);
    void Update(TUserId userId, Locale locale);
    Locale? GetFromRepository(TUserId userId);
}