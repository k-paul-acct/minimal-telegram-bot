using System.Reflection;

namespace MinimalTelegramBot.Handling.Filters;

public sealed class BotRequestFilterFactoryContext
{
    public required MethodInfo MethodInfo { get; init; }
    public required IServiceProvider Services { get; init; }
}
