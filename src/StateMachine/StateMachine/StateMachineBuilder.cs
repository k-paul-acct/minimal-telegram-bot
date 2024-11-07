using Microsoft.Extensions.DependencyInjection;

namespace MinimalTelegramBot.StateMachine;

internal sealed class StateMachineBuilder : IStateMachineBuilder
{
    public StateMachineBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }
}
