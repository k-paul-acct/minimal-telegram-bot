using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.Handling;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Extensions;

// TODO: Docs.
public static class ServiceCollectionExtensions
{
    public static IStateMachineBuilder AddStateMachine(this IServiceCollection services, Action<StateManagementOptions>? configureOptions = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.Configure<StateManagementOptions>(options =>
        {
            configureOptions?.Invoke(options);
        });

        services.PostConfigure((StateManagementOptions options) =>
        {
            options.Repository ??= new InMemoryStateRepository();
            options.TypeInfoResolver ??= new ReflectionStateTypeInfoResolver();
            options.Serializer ??= new StateSerializer(options.TypeInfoResolver, options.SerializerOptions);
        });

        services.AddSingleton<IConfigureOptions<HandlerDelegateBuilderOptions>>(configureServices =>
        {
            var stateManagementOptions = configureServices.GetRequiredService<IOptions<StateManagementOptions>>();
            if (stateManagementOptions.Value.TypeInfoResolver is null)
            {
                return new ConfigureOptions<HandlerDelegateBuilderOptions>(_ =>
                {
                });
            }

            var interceptor = new StateParameterHandlerDelegateBuilderInterceptor(stateManagementOptions.Value.TypeInfoResolver);

            return new ConfigureOptions<HandlerDelegateBuilderOptions>(options => options.Interceptors.Add(interceptor));
        });

        services.TryAddSingleton<IStateMachine, StateMachine>();

        return new StateMachineBuilder(services);
    }
}
