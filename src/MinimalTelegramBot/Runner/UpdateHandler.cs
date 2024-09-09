using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.Logging;
using MinimalTelegramBot.StateMachine.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Runner;

internal sealed class UpdateHandler
{
    private readonly ITelegramBotClient _client;
    private readonly IBotRequestContextAccessor _contextAccessor;
    private readonly InfrastructureLogger _logger;
    private readonly BotRequestDelegate _pipeline;
    private readonly IServiceProvider _services;

    public UpdateHandler(IServiceProvider services, BotRequestDelegate pipeline, InfrastructureLogger logger)
    {
        _services = services;
        _pipeline = pipeline;
        _logger = logger;
        _client = _services.GetRequiredService<ITelegramBotClient>();
        _contextAccessor = _services.GetRequiredService<IBotRequestContextAccessor>();
    }

    public async Task Handle(Update update)
    {
        await using var scope = _services.CreateAsyncScope();

        var context = new BotRequestContext();

        _contextAccessor.BotRequestContext = context;

        var chatId = update.Message?.Chat.Id ??
                     update.CallbackQuery?.Message?.Chat.Id ??
                     update.EditedMessage?.Chat.Id ??
                     update.ChannelPost?.Chat.Id ??
                     update.EditedChannelPost?.Chat.Id ??
                     update.MessageReaction?.Chat.Id ??
                     update.MessageReactionCount?.Chat.Id ??
                     update.ChatBoost?.Chat.Id ??
                     update.RemovedChatBoost?.Chat.Id ??
                     0;

        var messageText = update.Message?.Text;
        var callbackData = update.CallbackQuery?.Data;

        context.Client = _client;
        context.Update = update;
        context.ChatId = chatId;
        context.MessageText = messageText;
        context.CallbackData = callbackData;
        context.Services = scope.ServiceProvider;

        try
        {
            var localeProvider = scope.ServiceProvider.GetService<IUserLocaleProvider>();
            if (localeProvider is not null)
            {
                var locale = await localeProvider.GetUserLocaleAsync(chatId);
                context.UserLocale = locale;
            }

            var stateMachine = scope.ServiceProvider.GetService<IStateMachine>();
            if (stateMachine is not null)
            {
                var state = stateMachine.GetState(chatId);
                context.UserState = state;
            }

            await _pipeline(context);
        }
        catch (Exception ex)
        {
            _logger.ApplicationError(ex);
        }
    }
}
