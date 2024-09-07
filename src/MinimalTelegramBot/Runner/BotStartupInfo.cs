namespace MinimalTelegramBot.Runner;

internal sealed class BotStartupInfo
{
    public long BotId { get; set; }
    public string BotUsername { get; set; } = null!;
    public string BotFirstName { get; set; } = null!;
    public string? BotLastName { get; set; }
    public string BotFullName => BotLastName is null ? BotFirstName : $"{BotFirstName} {BotLastName}";
}
