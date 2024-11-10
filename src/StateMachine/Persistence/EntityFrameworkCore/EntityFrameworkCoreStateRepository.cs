using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

internal sealed class EntityFrameworkCoreStateRepository<TContext, TEntity> : IStateRepository
    where TContext : DbContext, IStateMachineDbContext<TEntity>
    where TEntity : class, IMinimalTelegramBotState, new()
{
    private readonly ILogger<EntityFrameworkCoreStateRepository<TContext, TEntity>> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreStateRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger<EntityFrameworkCoreStateRepository<TContext, TEntity>>>();
    }

    public async ValueTask<StateEntry?> GetState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        _logger.LogInformation("Getting state with context {StateEntryContext}.", entryContext);

        var state = await dbContext.MinimalTelegramBotStates.AsNoTracking().FirstOrDefaultAsync(
            x => x.UserId == entryContext.UserId &&
                 x.ChatId == entryContext.ChatId &&
                 x.MessageThreadId == entryContext.MessageThreadId,
            cancellationToken);

        return state is null
            ? new StateEntry?()
            : new StateEntry(new StateTypeInfo(state.StateGroupName, state.StateId), state.StateData);
    }

    public async ValueTask SetState(StateEntryContext entryContext, StateEntry entry, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        _logger.LogInformation("Setting state with context {StateEntryContext}.", entryContext);

        var state = await dbContext.MinimalTelegramBotStates.FirstOrDefaultAsync(
            x => x.UserId == entryContext.UserId &&
                 x.ChatId == entryContext.ChatId &&
                 x.MessageThreadId == entryContext.MessageThreadId,
            cancellationToken);

        if (state is null)
        {
            var newState = new TEntity
            {
                UserId = entryContext.UserId,
                ChatId = entryContext.ChatId,
                MessageThreadId = entryContext.MessageThreadId,
                StateGroupName = entry.TypeInfo.StateGroupName,
                StateId = entry.TypeInfo.StateId,
                StateData = entry.StateData,
            };

            dbContext.MinimalTelegramBotStates.Add(newState);
        }
        else
        {
            state.StateGroupName = entry.TypeInfo.StateGroupName;
            state.StateId = entry.TypeInfo.StateId;
            state.StateData = entry.StateData;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        _logger.LogInformation("Deleting state with context {StateEntryContext}.", entryContext);

        await dbContext.MinimalTelegramBotStates
            .Where(x => x.UserId == entryContext.UserId &&
                        x.ChatId == entryContext.ChatId &&
                        x.MessageThreadId == entryContext.MessageThreadId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
