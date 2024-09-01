using Telegram.Bot;
using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Settings;

public class BotApplicationBuilderOptions
{
    public string[]? Args { get; set; }
    public string? Token { get; set; }
    public ReceiverOptions? ReceiverOptions { get; set; }
    internal Action<TelegramBotClientOptions>? TelegramBotClientOptionsConfigure { get; set; }

    internal void Validate()
    {
        if (Token is null)
        {
            throw new Exception("No bot token specified");
        }
    }
}