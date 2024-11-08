using Telegram.Bot.Types;

namespace MinimalTelegramBot.Settings;

/// <summary>
/// </summary>
public sealed class WebhookOptions
{
    /// <summary>
    /// </summary>
    /// <param name="url"></param>
    public WebhookOptions(string url)
    {
        ArgumentNullException.ThrowIfNull(url);
        Url = url;
    }

    /// <summary>
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// </summary>
    public InputFileStream? Certificate { get; set; }

    /// <summary>
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// </summary>
    public int? MaxConnections { get; set; }

    /// <summary>
    /// </summary>
    public string? SecretToken { get; set; }
}
