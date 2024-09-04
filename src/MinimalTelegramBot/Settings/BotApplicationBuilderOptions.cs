using Telegram.Bot;
using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Settings;

internal sealed class BotApplicationBuilderOptions
{
    public string[]? Args { get; set; }
    public string? Token { get; set; }
    public HostApplicationBuilderSettings? HostApplicationBuilderSettings { get; set; }
    public ReceiverOptions ReceiverOptions { get; set; } = new();

    public Action<TelegramBotClientOptions>? TelegramBotClientOptionsConfigure { get; set; }

    public void Validate()
    {
        if (Token is null)
        {
            throw new Exception("No bot token specified");
        }
    }
}