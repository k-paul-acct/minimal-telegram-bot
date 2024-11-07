using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

internal sealed class EntityFrameworkCoreRepository<TContext, TEntity> : IUserStateRepository
    where TContext : DbContext, IStateMachineDbContext<TEntity>
    where TEntity : class, IMinimalTelegramBotState
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
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        var state = await context.MinimalTelegramBotStates.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        return default!;
    }

    public ValueTask SetState<TState>(long userId, TState state, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask DeleteState(long userId, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        _logger.LogInformation("Deleting state of user with ID {UserId}.'", userId);
        await context.MinimalTelegramBotStates.AsNoTracking()
            .Where(x => x.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
