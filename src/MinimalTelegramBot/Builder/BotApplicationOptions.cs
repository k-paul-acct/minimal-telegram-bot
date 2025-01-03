using Microsoft.AspNetCore.Builder;

namespace MinimalTelegramBot.Builder;

// TODO: Docs.
/// <summary>
/// </summary>
public sealed class BotApplicationOptions
{
    /// <summary>
    /// </summary>
    public string? Token { get; init; }

    /// <summary>
    /// </summary>
    public WebApplicationOptions? WebApplicationOptions { get; init; }
}
