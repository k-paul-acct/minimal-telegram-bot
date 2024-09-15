namespace MinimalTelegramBot.Handling;

public sealed class UpdateHandlingRequirement
{
    public UpdateHandlingRequirement(object requirement)
    {
        Requirement = requirement;
    }

    public object Requirement { get; }
}
