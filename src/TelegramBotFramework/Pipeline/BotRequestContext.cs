using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotFramework.Pipeline;

public class BotRequestContext
{
    public ITelegramBotClient Client { get; init; } = null!;
    public Update Update { get; init; } = null!;
    public IServiceProvider ServiceProvider { get; init; } = null!;
    public bool UpdateHandlingStarted { get; set; }
    public IDictionary<string, object?> Items { get; } = new Dictionary<string, object?>();
}