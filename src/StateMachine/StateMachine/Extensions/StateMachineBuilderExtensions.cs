using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class StateMachineBuilderExtensions
{
    public static IStateMachineBuilder UseRepository<TRepository>(this IStateMachineBuilder builder)
        where TRepository : IUserStateRepository
    {
        builder.Services.AddSingleton<IConfigureOptions<StateManagementOptions>>(services =>
        {
            return new ConfigureOptions<StateManagementOptions>(options =>
            {
                options.Repository = ActivatorUtilities.CreateInstance<TRepository>(services);
            });
        });

        return builder;
    }

    public static IStateMachineBuilder UseRepository<TRepository>(this IStateMachineBuilder builder, TRepository repository)
        where TRepository : IUserStateRepository
    {
        builder.Services.Configure<StateManagementOptions>(options =>
        {
            options.Repository = repository;
        });

        return builder;
    }
}