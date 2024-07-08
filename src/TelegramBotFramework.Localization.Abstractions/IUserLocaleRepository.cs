namespace TelegramBotFramework.Localization.Abstractions;

public interface IUserLocaleRepository
{
    /// <summary>
    ///     Get user locale.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <returns>User locale if was specified before or null if no locale found.</returns>
    Locale? GetUserLocale(long userId);

    void SetUserLocale(long userId, Locale locale);
}