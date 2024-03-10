namespace TelegramBotFramework.Localization.Abstractions;

public interface IUserLocaleRepository<in TUserId>
{
    /// <summary>
    ///     Get user locale.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <returns>User locale if was specified before or null if no locale found.</returns>
    Locale? GetUserLocale(TUserId userId);

    void SetUserLocale(TUserId userId, Locale locale);
}