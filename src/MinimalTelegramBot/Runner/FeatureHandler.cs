using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Runner;

internal static class FeatureHandler
{
    public static Task HandleStartupFeatures(this IBotApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        return HandleDeleteWebhookOnStartupFeature(app, cancellationToken);
    }

    public static Task HandleShutdownFeatures(this IBotApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        return HandleDeleteWebhookOnShutdownFeature(app, cancellationToken);
    }

    private static Task HandleDeleteWebhookOnStartupFeature(IBotApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        return app.Properties.ContainsKey("__DeleteWebhookOnStartup")
            ? DeleteWebhook(app, cancellationToken)
            : Task.CompletedTask;
    }

    private static Task HandleDeleteWebhookOnShutdownFeature(IBotApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        return app.Properties.ContainsKey("__DeleteWebhookOnShutdown")
            ? DeleteWebhook(app, cancellationToken)
            : Task.CompletedTask;
    }

    private static Task DeleteWebhook(IBotApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        var client = app.Services.GetRequiredService<ITelegramBotClient>();
        var receiverOptions = app.Services.GetRequiredService<IOptions<ReceiverOptions>>().Value;
        return client.DeleteWebhook(receiverOptions.DropPendingUpdates, cancellationToken);
    }
}
