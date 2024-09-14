using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Settings;

internal sealed class BotApplicationBuilderOptions
{
    public string[] Args { get; set; } = [];
    public string? Token { get; set; }
    public HostApplicationBuilderSettings? HostApplicationBuilderSettings { get; set; }
    public Action<TelegramBotClientOptions>? TelegramBotClientOptionsConfigure { get; set; }

    public ReceiverOptions ReceiverOptions { get; set; } = new()
    {
        AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery,],
        DropPendingUpdates = false,
    };
}
