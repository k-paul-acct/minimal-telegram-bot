using Telegram.Bot.Types;

namespace MinimalTelegramBot.Settings;

public class WebhookOptions
{
    public required string Url { get; set; }
    public InputFileStream? Certificate { get; set; }
    public string? IpAddress { get; set; }
    public int? MaxConnections { get; set; }
    public string? SecretToken { get; set; }
}
