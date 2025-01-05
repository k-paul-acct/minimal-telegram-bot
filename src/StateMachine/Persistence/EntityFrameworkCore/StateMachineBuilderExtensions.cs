using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

/// <summary>
///     StateMachineBuilderExtensions.
/// </summary>
public static class StateMachineBuilderExtensions
{
    /// <summary>
    ///     Specifies the <see cref="IStateMachine"/> to persist states to the specified <see cref="DbContext"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IStateMachineBuilder"/>.</param>
    /// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
    /// <returns>The current instance of <see cref="IStateMachineBuilder"/>.</returns>
    public static IStateMachineBuilder PersistStatesToDbContext<TContext>(this IStateMachineBuilder builder)
        where TContext : DbContext, IStateMachineDbContext
    {
        builder.Services.AddSingleton<IConfigureOptions<StateManagementOptions>>(services =>
        {
            return new ConfigureOptions<StateManagementOptions>(options =>
            {
                options.Repository = new EntityFrameworkCoreStateRepository<TContext, MinimalTelegramBotState>(services);
            });
        });

        return builder;
    }

    /// <summary>
    ///     Specifies the <see cref="IStateMachine"/> to persist states to the specified <see cref="DbContext"/>
    ///     with a specified <see cref="IMinimalTelegramBotState"/> entity type.
    /// </summary>
    /// <param name="builder">The <see cref="IStateMachineBuilder"/>.</param>
    /// <typeparam name="TContext">The type of the <see cref="DbContext"/>.</typeparam>
    /// <typeparam name="TEntity">The type of the custom <see cref="IMinimalTelegramBotState"/> entity.</typeparam>
    /// <returns>The current instance of <see cref="IStateMachineBuilder"/>.</returns>
    public static IStateMachineBuilder PersistStatesToDbContext<TContext, TEntity>(this IStateMachineBuilder builder)
        where TContext : DbContext, IStateMachineDbContext<TEntity>
        where TEntity : class, IMinimalTelegramBotState, new()
    {
        builder.Services.AddSingleton<IConfigureOptions<StateManagementOptions>>(services =>
        {
            return new ConfigureOptions<StateManagementOptions>(options =>
            {
                options.Repository = new EntityFrameworkCoreStateRepository<TContext, TEntity>(services);
            });
        });

        return builder;
    }
}
