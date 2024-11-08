using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.Handling;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IStateMachineBuilder AddStateMachine(this IServiceCollection services, Action<StateManagementOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.Configure<StateManagementOptions>(options =>
        {
            options.StateSerializationOptions = StateSerializationOptions.Default;
            configureOptions?.Invoke(options);
        });

        services.PostConfigure((StateManagementOptions options) =>
        {
            options.StateTypeInfoResolver = new ReflectionStateTypeInfoResolver();
            options.Repository ??= new InMemoryUserStateRepository();
        });

        services.AddSingleton<IConfigureOptions<HandlerDelegateBuilderOptions>>(configureServices =>
        {
            var stateManagementOptions = configureServices.GetRequiredService<IOptions<StateManagementOptions>>();
            if (stateManagementOptions.Value.StateTypeInfoResolver is null)
            {
                return new ConfigureOptions<HandlerDelegateBuilderOptions>(_ =>
                {
                });
            }

            var interceptor = new StateParameterHandlerDelegateBuilderInterceptor(stateManagementOptions.Value.StateTypeInfoResolver);

            return new ConfigureOptions<HandlerDelegateBuilderOptions>(options => options.Interceptors.Add(interceptor));
        });

        services.TryAddSingleton<IStateMachine, StateMachine>();

        return new StateMachineBuilder(services);
    }
}
