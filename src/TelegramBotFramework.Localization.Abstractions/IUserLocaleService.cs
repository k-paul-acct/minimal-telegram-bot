namespace TelegramBotFramework.Localization.Abstractions;

public interface IUserLocaleService
{
    Task<Locale> UpdateWithProviderAsync(long userId);
    ValueTask<Locale> GetFromRepositoryOrUpdateWithProviderAsync(long userId);
    void Update(long userId, Locale locale);
    Locale? GetFromRepository(long userId);
}