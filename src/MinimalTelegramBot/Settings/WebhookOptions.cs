using Telegram.Bot.Types;

namespace MinimalTelegramBot.Settings;

public class WebhookOptions
{
    public required string Url { get; init; }
    public InputFileStream? Certificate { get; init; }
    public string? IpAddress { get; init; }
    public int? MaxConnections { get; init; }
    public string? SecretToken { get; init; }
}