using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Extensions;

/// <summary>
///     StateMachineBuilderExtensions.
/// </summary>
public static class StateMachineBuilderExtensions
{
    /// <summary>
    ///     Configures the <see cref="IStateMachine"/> to persist states to the specified repository.
    /// </summary>
    /// <remarks>
    ///     Note, that provided repository is meant to be a singleton.
    /// </remarks>
    /// <typeparam name="TRepository">The type of the repository.</typeparam>
    /// <param name="builder">The state machine builder.</param>
    /// <returns>The current instance of <see cref="IStateMachineBuilder"/>.</returns>
    public static IStateMachineBuilder PersistStatesToRepository<TRepository>(this IStateMachineBuilder builder)
        where TRepository : IStateRepository
    {
        builder.Services.AddSingleton<IConfigureOptions<StateManagementOptions>>(services =>
        {
            return new ConfigureOptions<StateManagementOptions>(options =>
            {
                options.Repository = ActivatorUtilities.CreateInstance<TRepository>(services);
            });
        });

        return builder;
    }

    /// <summary>
    ///     Configures the <see cref="IStateMachine"/> to persist states to the specified repository instance.
    /// </summary>
    /// <typeparam name="TRepository">The type of the repository.</typeparam>
    /// <param name="builder">The state machine builder.</param>
    /// <param name="repository">The repository instance.</param>
    /// <returns>The current instance of <see cref="IStateMachineBuilder"/>.</returns>
    public static IStateMachineBuilder PersistStatesToRepository<TRepository>(this IStateMachineBuilder builder, TRepository repository)
        where TRepository : IStateRepository
    {
        builder.Services.Configure<StateManagementOptions>(options =>
        {
            options.Repository = repository;
        });

        return builder;
    }

    /// <summary>
    ///     Configures the <see cref="IStateMachine"/> to use a <see cref="HybridCache"/> for caching.
    /// </summary>
    /// <param name="builder">The state machine builder.</param>
    /// <param name="cacheEntryOptions">The cache entry options for states.</param>
    /// <returns>The current instance of <see cref="IStateMachineBuilder"/>.</returns>
    public static IStateMachineBuilder WithHybridCache(this IStateMachineBuilder builder, HybridCacheEntryOptions? cacheEntryOptions = null)
    {
#pragma warning disable EXTEXP0018
        builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

        builder.Services.Configure<StateCachingOptions>(options =>
        {
            options.UseCaching = true;
            options.CacheEntryOptions = cacheEntryOptions;
        });

        return builder;
    }
}
