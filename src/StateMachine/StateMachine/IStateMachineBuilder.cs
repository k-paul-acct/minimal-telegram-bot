using Microsoft.Extensions.DependencyInjection;

namespace MinimalTelegramBot.StateMachine;

/// <summary>
///     Interface for building a state machine.
/// </summary>
public interface IStateMachineBuilder
{
    /// <summary>
    ///     Gets the service collection to configure the state machine.
    /// </summary>
    IServiceCollection Services { get; }
}
