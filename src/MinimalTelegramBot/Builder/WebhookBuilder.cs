using Microsoft.AspNetCore.Builder;
using MinimalTelegramBot.Settings;

namespace MinimalTelegramBot.Builder;

internal sealed class WebhookBuilder : IWebhookBuilder
{
    private readonly WebhookOptions _options;
    private string _listenPath;
    private WebApplication? _webApplication;
    private bool _webhookResponseEnabled;

    public WebhookBuilder(WebhookOptions options)
    {
        _options = options;
        _listenPath = "";
    }

    public IWebhookBuilder EnableWebhookResponse()
    {
        _webhookResponseEnabled = true;
        return this;
    }

    public IWebhookBuilder ListenOnPath(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        _listenPath = path;
        return this;
    }

    public IWebhookBuilder UseWebApplication(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);
        _webApplication = app;
        return this;
    }

    public WebhookConfiguration Build()
    {
        return new WebhookConfiguration
        {
            Options = _options,
            ListenPath = _listenPath,
            WebApplication = _webApplication,
            WebhookResponseEnabled = _webhookResponseEnabled,
        };
    }
}
