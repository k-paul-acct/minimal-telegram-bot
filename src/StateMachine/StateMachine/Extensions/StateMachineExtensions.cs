using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Extensions;

/// <summary>
///     StateMachineExtensions.
/// </summary>
public static class StateMachineExtensions
{
    /// <summary>
    ///     Gets the state for the <see cref="StateEntryContext"/> of the current <see cref="BotRequestContext"/>.
    /// </summary>
    /// <param name="context">The current instance of <see cref="BotRequestContext"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <typeparam name="TState">The type of state to retrieve.</typeparam>
    /// <returns>
    ///     The <see cref="ValueTask"/> that represents the asynchronous operation,
    ///     containing the state or null if the is no state.
    /// </returns>
    public static ValueTask<TState?> GetState<TState>(this BotRequestContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        var options = context.Services.GetRequiredService<IOptions<StateManagementOptions>>().Value;
        var entryContext = context.Update.CreateStateEntryContext(options.TrackingStrategy);
        return stateMachine.GetState<TState>(entryContext, cancellationToken);
    }

    /// <summary>
    ///     Sets the state for the <see cref="StateEntryContext"/> of the current <see cref="BotRequestContext"/>.
    /// </summary>
    /// <param name="context">The current instance of <see cref="BotRequestContext"/>.</param>
    /// <param name="state">The state to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <typeparam name="TState">The type of state to set.</typeparam>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public static ValueTask SetState<TState>(this BotRequestContext context, TState state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(state);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        var options = context.Services.GetRequiredService<IOptions<StateManagementOptions>>().Value;
        var entryContext = context.Update.CreateStateEntryContext(options.TrackingStrategy);
        return stateMachine.SetState(entryContext, state, cancellationToken);
    }

    /// <summary>
    ///     Deletes the state for the <see cref="StateEntryContext"/> of the current <see cref="BotRequestContext"/>.
    /// </summary>
    /// <param name="context">The current instance of <see cref="BotRequestContext"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public static ValueTask DropState(this BotRequestContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        var options = context.Services.GetRequiredService<IOptions<StateManagementOptions>>().Value;
        var entryContext = context.Update.CreateStateEntryContext(options.TrackingStrategy);
        return stateMachine.DropState(entryContext, cancellationToken);
    }
}
