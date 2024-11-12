using Microsoft.Extensions.Caching.Hybrid;

namespace MinimalTelegramBot.StateMachine;

// TODO: Docs.
/// <summary>
/// </summary>
public sealed class StateCachingOptions
{
    /// <summary>
    /// </summary>
    public bool UseCaching { get; set; }

    /// <summary>
    /// </summary>
    public HybridCacheEntryOptions? CacheEntryOptions { get; set; }
}
