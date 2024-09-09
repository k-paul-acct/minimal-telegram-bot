using MinimalTelegramBot.Logging;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Runner;

internal sealed class UpdateHandler
{
    private readonly InfrastructureLogger _logger;
    private readonly BotRequestDelegate _pipeline;
    private readonly IServiceProvider _services;
    private readonly IBotRequestContextFactory _contextFactory;

    public UpdateHandler(IServiceProvider services, BotRequestDelegate pipeline, InfrastructureLogger logger)
    {
        _services = services;
        _pipeline = pipeline;
        _logger = logger;
        _contextFactory = _services.GetRequiredService<IBotRequestContextFactory>();
    }

    public async Task<UpdateHandlingInvocationContext?> CreateInvocationContext(Update update,  bool webhookResponseAvailable)
    {
        BotRequestContext context;
        var scope = _services.CreateAsyncScope();

        try
        {
            context = await _contextFactory.Create(scope.ServiceProvider, update, webhookResponseAvailable);
        }
        catch (Exception ex)
        {
            await scope.DisposeAsync();
            _logger.ApplicationError(ex);
            return null;
        }

        return new UpdateHandlingInvocationContext
        {
            BotRequestContext = context,
            ServiceScope = scope,
            Pipeline = _pipeline,
            Logger = _logger,
        };
    }
}
