using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Settings;

public class BotApplicationBuilderOptions
{
    public string[]? Args { get; set; }
    public string? Token { get; set; }
    public ReceiverOptions? ReceiverOptions { get; set; }

    internal void Validate()
    {
        if (Token is null)
        {
            throw new Exception("No bot token specified");
        }
    }
}