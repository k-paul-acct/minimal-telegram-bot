namespace TelegramBotFramework;

public interface IBotApplicationBuilder
{
    IDictionary<string, object?> Properties { get; }
    IBotApplicationBuilder Use(Func<Func<BotRequestContext, Task>, Func<BotRequestContext, Task>> pipe);
    Func<BotRequestContext, Task> Build();
}