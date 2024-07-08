namespace MinimalTelegramBot;

public interface IBotRequestContextAccessor
{
    BotRequestContext? BotRequestContext { get; set; }
}