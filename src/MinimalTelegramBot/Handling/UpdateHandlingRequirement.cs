namespace MinimalTelegramBot.Handling;

/// <summary>
///     Represents a requirement for handling certain bot update.
/// </summary>
public sealed class UpdateHandlingRequirement
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UpdateHandlingRequirement"/>.
    /// </summary>
    /// <param name="requirement">The requirement object for handling updates.</param>
    public UpdateHandlingRequirement(object requirement)
    {
        Requirement = requirement;
    }

    /// <summary>
    ///     Gets the requirement object for handling updates.
    /// </summary>
    public object Requirement { get; }
}
