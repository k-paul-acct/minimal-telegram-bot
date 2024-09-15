using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling.Requirements;

public sealed class UpdateTypeRequirement
{
    public UpdateTypeRequirement(UpdateType updateType)
    {
        UpdateType = updateType;
    }

    public UpdateType UpdateType { get; }
}
