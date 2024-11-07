using Microsoft.Extensions.DependencyInjection;

namespace MinimalTelegramBot.StateMachine;

public interface IStateMachineBuilder
{
    IServiceCollection Services { get; }
}
