using Microsoft.AspNetCore.Builder;
using MinimalTelegramBot.Settings;

namespace MinimalTelegramBot.Builder;

internal sealed class WebhookConfiguration
{
    public required WebhookOptions Options { get; set; }
    public WebApplication? WebApplication { get; set; }
    public bool WebhookResponseEnabled { get; set; }
    public required string ListenPath { get; set; }
}
