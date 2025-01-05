namespace MinimalTelegramBot.Client;

/// <inheritdoc cref="Telegram.Bot.TelegramBotClientOptions"/>
public sealed class TelegramBotClientOptions
{
    /// <inheritdoc cref="Telegram.Bot.TelegramBotClientOptions.Token"/>
    public string? Token { get; set; }

    /// <inheritdoc cref="Telegram.Bot.TelegramBotClientOptions.BaseUrl"/>
    public string? BaseUrl { get; set; }

    /// <inheritdoc cref="Telegram.Bot.TelegramBotClientOptions.UseTestEnvironment"/>
    public bool UseTestEnvironment { get; set; }

    /// <inheritdoc cref="Telegram.Bot.TelegramBotClientOptions.RetryThreshold"/>
    public int RetryThreshold { get; set; } = 60;

    /// <inheritdoc cref="Telegram.Bot.TelegramBotClientOptions.RetryCount"/>
    public int RetryCount { get; set; } = 3;
}
