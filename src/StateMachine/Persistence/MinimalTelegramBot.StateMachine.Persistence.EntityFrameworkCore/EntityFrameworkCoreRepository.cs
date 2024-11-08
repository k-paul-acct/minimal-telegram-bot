using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

internal sealed class EntityFrameworkCoreRepository<TContext, TEntity> : IUserStateRepository
    where TContext : DbContext, IStateMachineDbContext<TEntity>
    where TEntity : class, IMinimalTelegramBotState, new()
{
    private readonly ILogger<EntityFrameworkCoreRepository<TContext, TEntity>> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger<EntityFrameworkCoreRepository<TContext, TEntity>>>();
    }

    public async ValueTask<TState?> GetState<TState>(long userId, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        var stateManagementOptions = scope.ServiceProvider.GetRequiredService<IOptions<StateManagementOptions>>().Value;

        _logger.LogInformation("Getting state of user with ID {UserId}.", userId);

        var state = await dbContext.MinimalTelegramBotStates.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (state is null)
        {
            return default;
        }

        var entry = new StateEntry(state.StateGroupName, state.StateId);
        var serialized = new SerializedState(entry, state.StateData);
        var stateTypeInfoResolver = stateManagementOptions.StateTypeInfoResolver ?? EmptyStateTypeInfoResolver.Default;

        return StateSerializer.Deserialize<TState>(serialized, stateTypeInfoResolver, stateManagementOptions.StateSerializationOptions);
    }

    public async ValueTask SetState<TState>(long userId, TState state, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        var stateManagementOptions = scope.ServiceProvider.GetRequiredService<IOptions<StateManagementOptions>>().Value;

        _logger.LogInformation("Setting state of user with ID {UserId}.", userId);

        var stateTypeInfoResolver = stateManagementOptions.StateTypeInfoResolver ?? EmptyStateTypeInfoResolver.Default;
        var serialised = StateSerializer.Serialize(state, stateTypeInfoResolver, stateManagementOptions.StateSerializationOptions);
        var existing = await dbContext.MinimalTelegramBotStates.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (existing is not null)
        {
            existing.StateGroupName = serialised.StateEntry.StateGroupName;
            existing.StateId = serialised.StateEntry.StateId;
            existing.StateData = serialised.StateData;
        }
        else
        {
            var toAdd = new TEntity
            {
                UserId = userId,
                StateGroupName = serialised.StateEntry.StateGroupName,
                StateId = serialised.StateEntry.StateId,
                StateData = serialised.StateData,
            };

            dbContext.MinimalTelegramBotStates.Add(toAdd);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteState(long userId, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        _logger.LogInformation("Deleting state of user with ID {UserId}.", userId);
        await dbContext.MinimalTelegramBotStates.Where(x => x.UserId == userId).ExecuteDeleteAsync(cancellationToken);
    }

    private sealed class EmptyStateTypeInfoResolver : IStateTypeInfoResolver
    {
        public static readonly EmptyStateTypeInfoResolver Default = new(default, default);

        private readonly StateEntry _stateEntry;
        private readonly Type? _type;

        private EmptyStateTypeInfoResolver(StateEntry stateEntry, Type? type)
        {
            _stateEntry = stateEntry;
            _type = type;
        }

        public bool GetInfo(Type type, out StateEntry stateEntry)
        {
            stateEntry = _stateEntry;
            return false;
        }

        public bool GetInfo(StateEntry stateEntry, [NotNullWhen(true)] out Type? stateType)
        {
            stateType = _type;
            return false;
        }
    }
}
