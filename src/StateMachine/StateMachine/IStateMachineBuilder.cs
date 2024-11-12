using Microsoft.Extensions.DependencyInjection;

namespace MinimalTelegramBot.StateMachine;

// TODO: Docs.
public interface IStateMachineBuilder
{
    IServiceCollection Services { get; }
}
