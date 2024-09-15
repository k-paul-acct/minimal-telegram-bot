using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.StateMachine.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot;

public sealed class BotRequestContext
{
    private readonly List<IDisposable> _disposables;
    internal readonly IDictionary<string, object?> _properties;

    internal BotRequestContext(IServiceProvider services, Update update, ITelegramBotClient client, IDictionary<string, object?> properties)
    {
        Services = services;
        Update = update;
        Client = client;
        _properties = properties;
        _disposables = [];
        UserLocale = Locale.Default;
        Data = new Dictionary<string, object?>();

        var messageText = update.Message?.Text;
        var callbackData = update.CallbackQuery?.Data;
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

        MessageText = messageText;
        CallbackData = callbackData;
        ChatId = chatId;
    }

    public ITelegramBotClient Client { get; }
    public Update Update { get; }
    public long ChatId { get; }
    public string? MessageText { get; }
    public string? CallbackData { get; }
    public IServiceProvider Services { get; }
    public IDictionary<string, object?> Data { get; }
    public Locale UserLocale { get; set; }
    public State? UserState { get; set; }

    public void RegisterForDispose(IDisposable disposable)
    {
        _disposables.Add(disposable);
    }

    internal void DisposeItems()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }

    internal async ValueTask DisposeItemsAsync()
    {
        foreach (var disposable in _disposables)
        {
            if (disposable is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else
            {
                disposable.Dispose();
            }
        }
    }
}
