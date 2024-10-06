using System.Reflection;

namespace MinimalTelegramBot.Handling.Filters;

/// <summary>
/// </summary>
public sealed class BotRequestFilterFactoryContext
{
    /// <summary>
    /// </summary>
    public required MethodInfo MethodInfo { get; init; }

    /// <summary>
    /// </summary>
    public required IServiceProvider Services { get; init; }
}
