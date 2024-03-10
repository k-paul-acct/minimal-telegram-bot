namespace TelegramBotFramework;

public interface IBotApplicationBuilder
{
    IDictionary<string, object?> Properties { get; }
    IBotApplicationBuilder Use(Func<BotRequestDelegate, BotRequestDelegate> pipe);
    BotRequestDelegate Build();
}