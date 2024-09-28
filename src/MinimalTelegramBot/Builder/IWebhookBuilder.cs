using Microsoft.AspNetCore.Builder;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     Interface for building webhook configuration.
/// </summary>
public interface IWebhookBuilder
{
    /// <summary>
    ///     Enables the direct webhook response to incoming updates.
    /// </summary>
    /// <returns>The current instance of <see cref="IWebhookBuilder"/>.</returns>
    IWebhookBuilder EnableWebhookResponse();

    /// <summary>
    ///     Configures the webhook to listen on the specified path.
    /// </summary>
    /// <param name="path">The path on which the webhook will listen.</param>
    /// <returns>The current instance of <see cref="IWebhookBuilder"/>.</returns>
    IWebhookBuilder ListenOnPath(string path);

    /// <summary>
    ///     Configures the webhook to be deleted when the application shuts down.
    /// </summary>
    /// <returns>The current instance of <see cref="IWebhookBuilder"/>.</returns>
    IWebhookBuilder DeleteWebhookOnShutdown();

    /// <summary>
    ///     Configures the webhook to use the custom web application.
    /// </summary>
    /// <param name="app">The web application to use for the webhook.</param>
    /// <returns>The current instance of <see cref="IWebhookBuilder"/>.</returns>
    IWebhookBuilder UseWebApplication(WebApplication app);
}
