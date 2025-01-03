namespace MinimalTelegramBot.Client;

// TODO: Docs.
/// <summary>
/// </summary>
public sealed class TelegramBotClientOptions
{
    /// <summary>
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    /// </summary>
    public bool UseTestEnvironment { get; set; }

    /// <summary>
    /// </summary>
    public int RetryThreshold { get; set; } = 60;

    /// <summary>
    /// </summary>
    public int RetryCount { get; set; } = 3;
}
