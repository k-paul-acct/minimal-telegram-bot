using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling.Requirements;

// TODO: Docs.
/// <summary>
/// </summary>
public sealed class UpdateTypeRequirement
{
    /// <summary>
    /// </summary>
    /// <param name="updateType"></param>
    public UpdateTypeRequirement(UpdateType updateType)
    {
        UpdateType = updateType;
    }

    /// <summary>
    /// </summary>
    public UpdateType UpdateType { get; }
}
