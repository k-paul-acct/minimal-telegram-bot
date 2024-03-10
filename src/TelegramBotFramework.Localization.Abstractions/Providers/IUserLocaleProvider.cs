namespace TelegramBotFramework.Localization.Abstractions.Providers;

public interface IUserLocaleProvider<in TUserId>
{
    Task<Locale> GetUserLocaleAsync(TUserId userId);
}