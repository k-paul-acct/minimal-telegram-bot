using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

    public async ValueTask<SerializedState?> GetState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        _logger.LogInformation("Getting state with context {StateEntryContext}.", stateEntryContext);

        var (entity, serializedStates) = await GetUserStates(dbContext, stateEntryContext.UserId, cancellationToken);
        return entity is null ? default : serializedStates.FirstOrDefault(x => x.StateEntryContext == stateEntryContext);
    }

    public async ValueTask SetState(SerializedState serializedState, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(serializedState);

        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        _logger.LogInformation("Setting state with context {StateEntryContext}.", serializedState.StateEntryContext);

        var (entity, serializedStates) = await GetUserStates(dbContext, serializedState.StateEntryContext.UserId, cancellationToken);

        if (entity is null)
        {
            SerializedState[] states = [serializedState,];
            var newEntity = new TEntity
            {
                UserId = serializedState.StateEntryContext.UserId,
                States = JsonSerializer.Serialize(states),
            };

            dbContext.MinimalTelegramBotStates.Add(newEntity);
        }
        else
        {
            var result = serializedStates
                .Where(x => x.StateEntryContext != serializedState.StateEntryContext)
                .Append(serializedState);

            entity.States = JsonSerializer.Serialize(result);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteState(StateEntryContext stateEntryContext, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        _logger.LogInformation("Deleting state with context {StateEntryContext}.", stateEntryContext);

        var (entity, serializedStates) = await GetUserStates(dbContext, stateEntryContext.UserId, cancellationToken);
        if (entity is null)
        {
            return;
        }

        var result = serializedStates.Where(x => x.StateEntryContext != stateEntryContext);

        entity.States = JsonSerializer.Serialize(result);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static async Task<(TEntity?, SerializedState[])> GetUserStates(TContext dbContext, long userId, CancellationToken cancellationToken)
    {
        var entity = await dbContext.MinimalTelegramBotStates.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (entity is null)
        {
            return (null, []);
        }

        var serialized = JsonSerializer.Deserialize<SerializedState[]>(entity.States) ?? [];
        return (entity, serialized);
    }
}
