using System.Reflection;

namespace MinimalTelegramBot.Handling.Filters;

/// <summary>
///     Context for creating bot request filter pipelines.
/// </summary>
public sealed class BotRequestFilterFactoryContext
{
    /// <summary>
    ///     Gets the method information for the bot request handler.
    /// </summary>
    public required MethodInfo MethodInfo { get; init; }

    /// <summary>
    ///     Gets the <see cref="IServiceProvider"/> used to resolve services.
    /// </summary>
    public required IServiceProvider Services { get; init; }
}
