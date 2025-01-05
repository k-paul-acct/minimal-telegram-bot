using MinimalTelegramBot.Settings;

namespace MinimalTelegramBot.Builder;

internal sealed class WebhookConfiguration
{
    public required WebhookOptions Options { get; set; }
    public bool SkipWebhookSettingOnStartup { get; set; }
    public bool DeleteWebhookOnShutdown { get; set; }
    public bool WebhookResponseEnabled { get; set; }
    public required string ListenPath { get; set; }
}
