namespace MinimalTelegramBot.Localization.Abstractions;

/// <summary>
///     Provides an interface for retrieving user locale information.
/// </summary>
public interface IUserLocaleProvider
{
    /// <summary>
    ///     Retrieves the locale information for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose locale information is being retrieved.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    ///     A task that represents an asynchronous operation. The task result contains the locale information for the specified user.
    /// </returns>
    Task<Locale> GetUserLocaleAsync(long userId, CancellationToken cancellationToken = default);
}
