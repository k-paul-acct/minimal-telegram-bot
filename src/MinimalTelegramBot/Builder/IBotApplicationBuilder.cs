namespace MinimalTelegramBot.Builder;

public interface IBotApplicationBuilder
{
    IServiceProvider Services { get; }
    IDictionary<string, object?> Properties { get; }
    IBotApplicationBuilder Use(Func<BotRequestDelegate, BotRequestDelegate> pipe);
    BotRequestDelegate Build();
}
