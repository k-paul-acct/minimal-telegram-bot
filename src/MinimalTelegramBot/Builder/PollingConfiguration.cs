using Microsoft.AspNetCore.Builder;

namespace MinimalTelegramBot.Builder;

internal sealed class PollingConfiguration
{
    public string? Url { get; set; }
    public Action<WebApplication>? StaticFilesAction { get; set; }
}
