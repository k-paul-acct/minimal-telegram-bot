using Microsoft.Extensions.Caching.Hybrid;

namespace MinimalTelegramBot.StateMachine;

/// <summary>
///     Options for state caching in the state machine.
/// </summary>
public sealed class StateCachingOptions
{
    /// <summary>
    ///     Gets or sets a value indicating whether caching is used.
    /// </summary>
    public bool UseCaching { get; set; }

    /// <summary>
    ///     Gets or sets the cache entry options for states for the <see cref="HybridCache"/>.
    /// </summary>
    public HybridCacheEntryOptions? CacheEntryOptions { get; set; }
}
