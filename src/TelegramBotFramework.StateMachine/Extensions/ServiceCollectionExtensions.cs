using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TelegramBotFramework.StateMachine.Abstractions;

namespace TelegramBotFramework.StateMachine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStateMachine(this IServiceCollection services)
    {
        services.TryAddSingleton<IUserStateRepository, InMemoryUserStateRepository>();
        services.TryAddScoped<IStateMachine, StateMachine>();
        return services;
    }
}