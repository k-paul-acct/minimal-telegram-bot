using Telegram.Bot.Polling;

namespace MinimalTelegramBot.Settings;

internal sealed class BotApplicationOptions
{
    public BotApplicationOptions(BotApplicationBuilderOptions builderOptions)
    {
        Token = builderOptions.Token ?? throw new Exception("Bot token not specified");
        ReceiverOptions = builderOptions.ReceiverOptions;
        Args = builderOptions.Args;
        BuilderOptions = builderOptions;
    }

    public string[]? Args { get; set; }
    public string Token { get; set; }
    public ReceiverOptions? ReceiverOptions { get; set; }
    public BotApplicationBuilderOptions? BuilderOptions { get; set; }
}
