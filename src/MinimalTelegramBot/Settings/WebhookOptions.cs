using Telegram.Bot.Types;

namespace MinimalTelegramBot.Settings;

/// <summary>
///     Options for configuring the webhook.
/// </summary>
public sealed class WebhookOptions
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WebhookOptions"/> class.
    /// </summary>
    /// <param name="url">HTTPS URL to send updates to. Use an empty string to remove webhook integration.</param>
    public WebhookOptions(string url)
    {
        ArgumentNullException.ThrowIfNull(url);
        Url = url;
    }

    /// <inheritdoc cref="Telegram.Bot.Requests.SetWebhookRequest.Url"/>
    public string Url { get; set; }

    /// <inheritdoc cref="Telegram.Bot.Requests.SetWebhookRequest.Certificate"/>
    public InputFileStream? Certificate { get; set; }

    /// <inheritdoc cref="Telegram.Bot.Requests.SetWebhookRequest.IpAddress"/>
    public string? IpAddress { get; set; }

    /// <inheritdoc cref="Telegram.Bot.Requests.SetWebhookRequest.MaxConnections"/>
    public int? MaxConnections { get; set; }

    /// <inheritdoc cref="Telegram.Bot.Requests.SetWebhookRequest.SecretToken"/>
    public string? SecretToken { get; set; }
}
