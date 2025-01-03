namespace MinimalTelegramBot.Runner;

internal sealed class BotStartupInfo
{
    public required long Id { get; init; }
    public required string FirstName { get; init; }
    public required string? LastName { get; init; }
    public required string Username { get; init; }
    public string FullName => LastName is null ? FirstName : $"{FirstName} {LastName}";
}
