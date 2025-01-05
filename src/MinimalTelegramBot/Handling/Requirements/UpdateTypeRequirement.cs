using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling.Requirements;

/// <summary>
///     Represents a requirement for a specific update type in the bot.
/// </summary>
public sealed class UpdateTypeRequirement
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UpdateTypeRequirement"/>.
    /// </summary>
    /// <param name="updateType">The type of update required to handle some update.</param>
    public UpdateTypeRequirement(UpdateType updateType)
    {
        UpdateType = updateType;
    }

    /// <summary>
    ///     Gets the type of update required.
    /// </summary>
    public UpdateType UpdateType { get; }
}
