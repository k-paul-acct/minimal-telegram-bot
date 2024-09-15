using Telegram.Bot.Types;

namespace MinimalTelegramBot.Settings;

public sealed class WebhookOptions
{
    public WebhookOptions(string url)
    {
        ArgumentNullException.ThrowIfNull(url);
        Url = url;
    }

    public string Url { get; set; }
    public InputFileStream? Certificate { get; set; }
    public string? IpAddress { get; set; }
    public int? MaxConnections { get; set; }
    public string? SecretToken { get; set; }
}
