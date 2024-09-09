using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling.Filters;

[AttributeUsage(AttributeTargets.Method)]
public sealed class UpdateTypeAttribute : Attribute
{
    public UpdateTypeAttribute(UpdateType updateType)
    {
        UpdateType = updateType;
    }

    public UpdateType UpdateType { get; }
}
