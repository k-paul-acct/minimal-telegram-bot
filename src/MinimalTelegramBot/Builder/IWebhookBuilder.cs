using Microsoft.AspNetCore.Builder;

namespace MinimalTelegramBot.Builder;

public interface IWebhookBuilder
{
    IWebhookBuilder EnableWebhookResponse();
    IWebhookBuilder ListenOnPath(string path);
    IWebhookBuilder DeleteWebhookOnShutdown();
    IWebhookBuilder UseWebApplication(WebApplication app);
}
