using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.StateMachine.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot;

/// <summary>
///     Represents the context of a bot request, containing information about the update, client, and other relevant data.
/// </summary>
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

    /// <summary>
    ///     Gets the Telegram Bot client.
    /// </summary>
    public ITelegramBotClient Client { get; }

    /// <summary>
    ///     Gets an incoming update from Telegram.
    /// </summary>
    public Update Update { get; }

    /// <summary>
    ///     Gets chat ID for current update.
    ///     Note, that it is not possible to get chat ID for each update, so in such cases the chat ID is 0.
    /// </summary>
    public long ChatId { get; }

    /// <summary>
    ///     Gets text of an incoming <see cref="Message"/> update.
    ///     Note, that it is not possible to get message text for each update, so in such cases the message text is null.
    /// </summary>
    public string? MessageText { get; }

    /// <summary>
    ///     Gets callback data of an incoming <see cref="CallbackQuery"/> update.
    ///     Note, that it is not possible to get callback data for each update, so in such cases the callback data is null.
    /// </summary>
    public string? CallbackData { get; }

    /// <summary>
    ///     Gets the <see cref="IServiceProvider"/> that provides access to the current update's service container.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    ///     Gets a key/value collection that can be used to share data within the scope of this bot request.
    /// </summary>
    public IDictionary<string, object?> Data { get; }

    /// <summary>
    ///     Gets the locale of the user associated with current bot request. Defaults to <see cref="Locale.Default"/> locale.
    /// </summary>
    public Locale UserLocale { get; set; }

    /// <summary>
    ///     Gets the state of the user associated with current bot request. Defaults to null.
    /// </summary>
    public State? UserState { get; set; }

    /// <summary>
    ///     Registers a disposable object to be disposed of after the current bot request is processed.
    /// </summary>
    /// <param name="disposable">The disposable object to register.</param>
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
