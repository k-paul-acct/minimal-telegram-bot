using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Settings;

internal class BotApplicationOptions
{
    public BotApplicationOptions(BotApplicationBuilderOptions builderOptions)
    {
        Token = builderOptions.Token ?? throw new Exception("Bot token not specified");
        ReceiverOptions = builderOptions.ReceiverOptions;
    }

    public string Token { get; set; }
    public ReceiverOptions? ReceiverOptions { get; set; }
}