using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStateMachine(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IUserStateRepository, InMemoryUserStateRepository>();
        services.TryAddScoped<IStateMachine, StateMachine>();
        return services;
    }
}