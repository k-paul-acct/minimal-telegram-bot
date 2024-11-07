using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
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
        var options = scope.ServiceProvider.GetRequiredService<IOptions<StateManagementOptions>>();

        _logger.LogInformation("Getting state of user with ID {UserId}.", userId);

        var state = await dbContext.MinimalTelegramBotStates.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (state is null)
        {
            return default;
        }

        var entry = new StateEntry(state.StateGroupName, state.StateId);
        var serialized = new SerializedState(entry, state.StateData);
        var serializerContext = options.Value.StateSerializerContext ?? new EmptyStateSerializerContext(null);

        return StateSerializer.Deserialize<TState>(serialized, serializerContext);
    }

    public async ValueTask SetState<TState>(long userId, TState state, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<StateManagementOptions>>();

        _logger.LogInformation("Setting state of user with ID {UserId}.", userId);

        var serializerContext = options.Value.StateSerializerContext ?? new EmptyStateSerializerContext(null);
        var serialised = StateSerializer.Serialize(state, serializerContext);
        var existing = await dbContext.MinimalTelegramBotStates.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        if (existing is not null)
        {
            existing.StateGroupName = serialised.StateEntry.StateGroupName;
            existing.StateId = serialised.StateEntry.StateId;
            existing.StateData = serialised.SerializedInfo;
        }
        else
        {
            var toAdd = new TEntity
            {
                UserId = userId,
                StateGroupName = serialised.StateEntry.StateGroupName,
                StateId = serialised.StateEntry.StateId,
                StateData = serialised.SerializedInfo,
            };

            dbContext.MinimalTelegramBotStates.Add(toAdd);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteState(long userId, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        _logger.LogInformation("Deleting state of user with ID {UserId}.", userId);
        await context.MinimalTelegramBotStates
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private sealed class EmptyStateSerializerContext : IStateSerializerContext
    {
        public EmptyStateSerializerContext(JsonSerializerOptions? jsonSerializerOptions)
        {
            JsonSerializerOptions = jsonSerializerOptions;
        }

        public JsonSerializerOptions? JsonSerializerOptions { get; }

        public bool GetInfo(Type type, out StateEntry stateEntry)
        {
            stateEntry = default;
            return false;
        }

        public bool GetInfo(StateEntry stateEntry, [NotNullWhen(true)] out Type? stateType)
        {
            stateType = default;
            return false;
        }
    }
}
