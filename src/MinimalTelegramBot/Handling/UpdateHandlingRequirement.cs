namespace MinimalTelegramBot.Handling;

/// <summary>
/// </summary>
public sealed class UpdateHandlingRequirement
{
    /// <summary>
    /// </summary>
    /// <param name="requirement"></param>
    public UpdateHandlingRequirement(object requirement)
    {
        Requirement = requirement;
    }

    /// <summary>
    /// </summary>
    public object Requirement { get; }
}
