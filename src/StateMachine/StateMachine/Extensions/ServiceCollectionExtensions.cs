using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalTelegramBot.Handling;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IStateMachineBuilder AddStateMachine(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var serializerContext = new ReflectionStateSerializerContext();
        var interceptor = new StateParameterHandlerDelegateBuilderInterceptor(serializerContext);

        services.Configure<HandlerDelegateBuilderOptions>(options => options.Interceptors.Add(interceptor));

        services.TryAddSingleton<IUserStateRepository, InMemoryUserStateRepository>();
        services.TryAddScoped<IStateMachine, StateMachine>();

        return new StateMachineBuilder(services);
    }
}
