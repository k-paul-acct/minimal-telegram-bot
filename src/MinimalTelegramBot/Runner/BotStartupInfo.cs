namespace MinimalTelegramBot.Runner;

internal sealed class BotStartupInfo
{
    public required long Id { get; set; }
    public required string FirstName { get; set; }
    public required string? LastName { get; set; }
    public required string Username { get; set; }
    public string FullName => LastName is null ? FirstName : $"{FirstName} {LastName}";
}
