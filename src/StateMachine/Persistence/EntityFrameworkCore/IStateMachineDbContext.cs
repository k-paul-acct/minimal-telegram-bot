using Microsoft.EntityFrameworkCore;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

// TODO: Docs.
public interface IStateMachineDbContext<TEntity>
    where TEntity : class, IMinimalTelegramBotState, new()
{
    DbSet<TEntity> MinimalTelegramBotStates { get; }
}

public interface IStateMachineDbContext : IStateMachineDbContext<MinimalTelegramBotState>;
