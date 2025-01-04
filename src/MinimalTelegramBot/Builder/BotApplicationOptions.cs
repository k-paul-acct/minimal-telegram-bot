using Microsoft.AspNetCore.Builder;

namespace MinimalTelegramBot.Builder;

/// <summary>
///     Options for configuring the <see cref="BotApplication"/>.
/// </summary>
public sealed class BotApplicationOptions
{
    /// <summary>
    ///     Gets or sets the bot token used to authenticate the bot with the Telegram API.
    /// </summary>
    public string? Token { get; init; }

    /// <summary>
    ///     Gets or sets the options for configuring the <see cref="WebApplication"/>.
    /// </summary>
    public WebApplicationOptions? WebApplicationOptions { get; init; }
}
