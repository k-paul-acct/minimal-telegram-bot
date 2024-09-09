using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.StateMachine.Abstractions;
using Telegram.Bot.Types;

namespace MinimalTelegramBot;

internal sealed class BotRequestContextFactory : IBotRequestContextFactory
{
    private readonly ITelegramBotClientFactory _botClientFactory;

    public BotRequestContextFactory(ITelegramBotClientFactory botClientFactory)
    {
        _botClientFactory = botClientFactory;
    }

    public async Task<BotRequestContext> Create(IServiceProvider services, Update update, bool webhookResponseAvailable)
    {
        var context = new BotRequestContext();

        HandleContextAccessorFeature(services, context);

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

        context.Client = _botClientFactory.Create(webhookResponseAvailable);
        context.Update = update;
        context.ChatId = chatId;
        context.MessageText = messageText;
        context.CallbackData = callbackData;
        context.Services = services;

        var localeProvider = services.GetService<IUserLocaleProvider>();
        if (localeProvider is not null)
        {
            var locale = await localeProvider.GetUserLocaleAsync(chatId);
            context.UserLocale = locale;
        }

        var stateMachine = services.GetService<IStateMachine>();
        if (stateMachine is not null)
        {
            var state = stateMachine.GetState(chatId);
            context.UserState = state;
        }

        return context;
    }

    private static void HandleContextAccessorFeature(IServiceProvider serviceProvider, BotRequestContext context)
    {
        var contextAccessor = serviceProvider.GetRequiredService<IBotRequestContextAccessor>();
        contextAccessor.BotRequestContext = context;
    }
}
