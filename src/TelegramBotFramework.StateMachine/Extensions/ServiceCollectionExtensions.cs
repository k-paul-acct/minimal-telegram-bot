using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TelegramBotFramework.Abstractions;
using TelegramBotFramework.StateMachine.Abstractions;

namespace TelegramBotFramework.StateMachine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStateMachine<TUserIdProvider>(this IServiceCollection services)
        where TUserIdProvider : IUserIdProvider<long>
    {
        services.AddSingleton(typeof(IUserStateRepository), typeof(InMemoryUserStateRepository));
        services.AddScoped<IStateMachine, StateMachine>();
        services.TryAddScoped(typeof(IUserIdProvider<long>), typeof(TUserIdProvider));
        return services;
    }
}