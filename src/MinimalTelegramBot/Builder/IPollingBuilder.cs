using Microsoft.AspNetCore.Builder;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     Interface for building polling configuration.
/// </summary>
public interface IPollingBuilder
{
    /// <summary>
    ///     Configures the application to serve static files.
    /// </summary>
    /// <param name="url">The base URL to be used as the base URL for the media URLs sent by the bot.</param>
    /// <returns>The current instance of <see cref="IPollingBuilder"/>.</returns>
    IPollingBuilder UseStaticFiles(string url);

    /// <summary>
    ///     Configures the application to serve static files.
    /// </summary>
    /// <param name="url">The base URL to be used as the base URL for the media URLs sent by the bot.</param>
    /// <param name="options">The options for serving static files.</param>
    /// <returns>The current instance of <see cref="IPollingBuilder"/>.</returns>
    IPollingBuilder UseStaticFiles(string url, StaticFileOptions options);
}
