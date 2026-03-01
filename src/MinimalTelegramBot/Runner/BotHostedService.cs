using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinimalTelegramBot.Logging;
using Telegram.Bot;

namespace MinimalTelegramBot.Runner;

internal sealed partial class BotHostedService : IHostedService
{
    private readonly BotApplicationContainer _applicationContainer;
    private readonly ILogger _logger;

    public BotHostedService(BotApplicationContainer applicationContainer, ILoggerFactory loggerFactory)
    {
        _applicationContainer = applicationContainer;
        _logger = InfrastructureLog.CreateLogger(loggerFactory);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var app = _applicationContainer.BotApplicationBuilder ??
                  throw new InvalidOperationException("Bot Application was not initialized.");

        await app.HandleStartupFeatures(cancellationToken);

        var isWebhook = app.Properties.ContainsKey("__WebhookEnabled");

        var dispatch = _applicationContainer.DispatchFunc ?? (_ => Task.CompletedTask);
        await dispatch(cancellationToken);

        var botInfo = await GetBotStartupInfo(app, cancellationToken);
        InfrastructureLog.GettingUpdateStarted(_logger, isWebhook ? "Webhook" : "Polling", botInfo.Username, botInfo.FullName, botInfo.Id);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var app = _applicationContainer.BotApplicationBuilder ??
                  throw new InvalidOperationException("Bot Application was not initialized.");

        await app.HandleShutdownFeatures(cancellationToken);
    }

    private static async Task<BotStartupInfo> GetBotStartupInfo(IBotApplicationBuilder app, CancellationToken cancellationToken = default)
    {
        var client = app.Services.GetRequiredService<ITelegramBotClient>();
        var bot = await client.GetMe(cancellationToken);
        return new BotStartupInfo
        {
            Id = bot.Id,
            FirstName = bot.FirstName,
            LastName = bot.LastName,
            Username = bot.Username ?? "N/A",
        };
    }
}
