using Microsoft.EntityFrameworkCore;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

public static class StateMachineBuilderExtensions
{
    public static IStateMachineBuilder PersistStatesToDbContext<TContext>(this IStateMachineBuilder builder)
        where TContext : DbContext, IStateMachineDbContext
    {
        return builder;
    }

    public static IStateMachineBuilder PersistStatesToDbContext<TContext, TEntity>(this IStateMachineBuilder builder)
        where TContext : DbContext, IStateMachineDbContext<TEntity>
        where TEntity : class, IMinimalTelegramBotState
    {
        return builder;
    }
}
