using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Extensions;

// TODO: Docs.
public static class StateMachineBuilderExtensions
{
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

    public static IStateMachineBuilder PersistStatesToRepository<TRepository>(this IStateMachineBuilder builder, TRepository repository)
        where TRepository : IStateRepository
    {
        builder.Services.Configure<StateManagementOptions>(options =>
        {
            options.Repository = repository;
        });

        return builder;
    }

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
