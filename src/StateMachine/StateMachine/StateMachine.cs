using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine;

internal sealed class StateMachine : IStateMachine
{
    private readonly StateCachingOptions _cacheOptions;
    private readonly IStateRepository _repository;
    private readonly IStateSerializer _serializer;
    private readonly IServiceProvider _serviceProvider;

    public StateMachine(IServiceProvider serviceProvider, IOptions<StateManagementOptions> stateManagementOptions, IOptions<StateCachingOptions> stateCachingOptions)
    {
        _serviceProvider = serviceProvider;
        _repository = stateManagementOptions.Value.Repository ?? EmptyStateRepository.Default;
        _serializer = stateManagementOptions.Value.Serializer ?? EmptyStateSerializer.Default;
        _cacheOptions = stateCachingOptions.Value;
    }

    public async ValueTask<TState?> GetState<TState>(StateEntryContext entryContext, CancellationToken cancellationToken = default)
    {
        StateEntry? entry;

        if (!_cacheOptions.UseCaching)
        {
            entry = await _repository.GetState(entryContext, cancellationToken);
            return entry is null ? default : _serializer.Deserialize<TState>(entry.Value);
        }

        var cache = _serviceProvider.GetRequiredService<HybridCache>();

        entry = await cache.GetOrCreateAsync(
            $"{entryContext.UserId:X}-{entryContext.ChatId:X}-{entryContext.MessageThreadId:X}",
            (EntryContext: entryContext, Service: this),
            static (state, ct) => state.Service._repository.GetState(state.EntryContext, ct),
            _cacheOptions.CacheEntryOptions,
            cancellationToken: cancellationToken);

        return entry is null ? default : _serializer.Deserialize<TState>(entry.Value);
    }

    public async ValueTask SetState<TState>(StateEntryContext entryContext, TState state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(state);

        var entry = _serializer.Serialize(state);
        await _repository.SetState(entryContext, entry, cancellationToken);

        if (!_cacheOptions.UseCaching)
        {
            return;
        }

        var cache = _serviceProvider.GetRequiredService<HybridCache>();

        await cache.SetAsync(
            $"{entryContext.UserId:X}-{entryContext.ChatId:X}-{entryContext.MessageThreadId:X}",
            (StateEntry?)entry,
            _cacheOptions.CacheEntryOptions,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DropState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteState(entryContext, cancellationToken);

        if (!_cacheOptions.UseCaching)
        {
            return;
        }

        var cache = _serviceProvider.GetRequiredService<HybridCache>();

        await cache.SetAsync(
            $"{entryContext.UserId:X}-{entryContext.ChatId:X}-{entryContext.MessageThreadId:X}",
            new StateEntry?(),
            _cacheOptions.CacheEntryOptions,
            cancellationToken: cancellationToken);
    }

    private sealed class EmptyStateRepository : IStateRepository
    {
        public static readonly EmptyStateRepository Default = new();

        public ValueTask<StateEntry?> GetState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
        {
            return default;
        }

        public ValueTask SetState(StateEntryContext entryContext, StateEntry entry, CancellationToken cancellationToken = default)
        {
            return default;
        }

        public ValueTask DeleteState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
        {
            return default;
        }
    }

    private sealed class EmptyStateSerializer : IStateSerializer
    {
        public static readonly EmptyStateSerializer Default = new();

        public StateEntry Serialize<TState>(TState state)
        {
            return default;
        }

        public TState? Deserialize<TState>(StateEntry entry)
        {
            return default;
        }
    }
}
