namespace MinimalTelegramBot.Localization.Abstractions;

public interface IUserLocaleProvider
{
    Task<Locale> GetUserLocaleAsync(long userId);
}