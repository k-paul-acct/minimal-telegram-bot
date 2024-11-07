using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

public static class StateMachineBuilderExtensions
{
    public static IStateMachineBuilder PersistStatesToDbContext<TContext>(this IStateMachineBuilder builder)
        where TContext : DbContext, IStateMachineDbContext
    {
        builder.Services.AddSingleton<IConfigureOptions<StateManagementOptions>>(services =>
        {
            return new ConfigureOptions<StateManagementOptions>(options =>
            {
                options.Repository = new EntityFrameworkCoreRepository<TContext, MinimalTelegramBotState>(services);
            });
        });

        return builder;
    }

    public static IStateMachineBuilder PersistStatesToDbContext<TContext, TEntity>(this IStateMachineBuilder builder)
        where TContext : DbContext, IStateMachineDbContext<TEntity>
        where TEntity : class, IMinimalTelegramBotState
    {
        builder.Services.AddSingleton<IConfigureOptions<StateManagementOptions>>(services =>
        {
            return new ConfigureOptions<StateManagementOptions>(options =>
            {
                options.Repository = new EntityFrameworkCoreRepository<TContext, TEntity>(services);
            });
        });

        return builder;
    }
}
