using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.StateMachine.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot;

public class BotRequestContext
{
    public ITelegramBotClient Client { get; set; } = null!;
    public Update Update { get; set; } = null!;
    public long ChatId { get; set; }
    public string? MessageText { get; set; }
    public string? CallbackData { get; set; }
    public IServiceProvider Services { get; set; } = null!;
    public IDictionary<string, object?> Data { get; } = new Dictionary<string, object?>();
    public Locale UserLocale { get; internal set; } = Locale.Default;
    public State? UserState { get; internal set; }

    internal bool UpdateHandlingStarted { get; set; }
}