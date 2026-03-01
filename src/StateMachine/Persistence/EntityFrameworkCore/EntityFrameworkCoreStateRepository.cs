using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

internal sealed partial class EntityFrameworkCoreStateRepository<TContext, TEntity> : IStateRepository
    where TContext : DbContext, IStateMachineDbContext<TEntity>
    where TEntity : class, IMinimalTelegramBotState, new()
{
    private readonly ILogger _logger;
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

        Log.GettingState(_logger, entryContext);

        var state = await dbContext.MinimalTelegramBotStates.AsNoTracking().FirstOrDefaultAsync(
            x => x.UserId == entryContext.UserId &&
                 x.ChatId == entryContext.ChatId &&
                 x.MessageThreadId == entryContext.MessageThreadId,
            cancellationToken);

        return state is null || state.StateData == ""
            ? new StateEntry?()
            : new StateEntry(new StateTypeInfo(state.StateGroupName, state.StateId), state.StateData);
    }

    public async ValueTask SetState(StateEntryContext entryContext, StateEntry entry, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        Log.SettingState(_logger, entryContext);

        var affected = await dbContext.MinimalTelegramBotStates
            .Where(x => x.UserId == entryContext.UserId &&
                        x.ChatId == entryContext.ChatId &&
                        x.MessageThreadId == entryContext.MessageThreadId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(state => state.StateGroupName, entry.TypeInfo.StateGroupName)
                .SetProperty(state => state.StateId, entry.TypeInfo.StateId)
                .SetProperty(state => state.StateData, entry.StateData), cancellationToken);

        if (affected != 0)
        {
            return;
        }

        dbContext.MinimalTelegramBotStates.Add(new TEntity
        {
            UserId = entryContext.UserId,
            ChatId = entryContext.ChatId,
            MessageThreadId = entryContext.MessageThreadId,
            StateGroupName = entry.TypeInfo.StateGroupName,
            StateId = entry.TypeInfo.StateId,
            StateData = entry.StateData,
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async ValueTask DeleteState(StateEntryContext entryContext, CancellationToken cancellationToken = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        Log.DeletingState(_logger, entryContext);

        await dbContext.MinimalTelegramBotStates
            .Where(x => x.UserId == entryContext.UserId &&
                        x.ChatId == entryContext.ChatId &&
                        x.MessageThreadId == entryContext.MessageThreadId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(state => state.StateData, ""), cancellationToken);
    }

    private static partial class Log
    {
        [LoggerMessage(1, LogLevel.Debug, "Getting state with context {stateEntryContext}")]
        public static partial void GettingState(ILogger logger, StateEntryContext stateEntryContext);

        [LoggerMessage(2, LogLevel.Debug, "Setting state with context {stateEntryContext}")]
        public static partial void SettingState(ILogger logger, StateEntryContext stateEntryContext);

        [LoggerMessage(3, LogLevel.Debug, "Deleting state with context {stateEntryContext}")]
        public static partial void DeletingState(ILogger logger, StateEntryContext stateEntryContext);
    }
}
