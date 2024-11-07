using System.Text.Json;
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

        services.Configure<StateManagementOptions>(_ =>
        {
        });

        services.PostConfigure((StateManagementOptions options) =>
        {
            options.Repository ??= new InMemoryUserStateRepository();
            options.JsonSerializerOptions = JsonSerializerOptions.Default;
        });

        services.TryAddSingleton<IStateMachine, StateMachine>();

        return new StateMachineBuilder(services);
    }
}
