using Microsoft.EntityFrameworkCore;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

/// <summary>
///     Represents a minimal database context for the state machine.
/// </summary>
/// <typeparam name="TEntity">The type of the state entity.</typeparam>
public interface IStateMachineDbContext<TEntity>
    where TEntity : class, IMinimalTelegramBotState, new()
{
    /// <summary>
    ///     Gets the <see cref="DbSet{TEntity}"/> of MinimalTelegramBotState entities.
    /// </summary>
    DbSet<TEntity> MinimalTelegramBotStates { get; }
}

/// <summary>
///     Represents a database context for the state machine with a default state entity type.
/// </summary>
public interface IStateMachineDbContext : IStateMachineDbContext<MinimalTelegramBotState>;
