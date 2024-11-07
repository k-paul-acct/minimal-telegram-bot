using Microsoft.Extensions.DependencyInjection;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class StateMachineExtensions
{
    /// <summary>
    ///     Gets the state of the current user in the <see cref="BotRequestContext"/>.
    /// </summary>
    /// <param name="context">The current instance of <see cref="BotRequestContext"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>
    ///     The <see cref="ValueTask"/> that represents the asynchronous operation,
    ///     containing the state of the user or null if the user has no state.
    /// </returns>
    public static ValueTask<TState?> GetState<TState>(this BotRequestContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        return stateMachine.GetState<TState>(context.ChatId, cancellationToken);
    }

    /*/// <summary>
    ///     Sets or updates the state of the current user in the <see cref="BotRequestContext"/>.
    /// </summary>
    /// <param name="context">The current instance of <see cref="BotRequestContext"/>.</param>
    /// <param name="state">The state to be set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public static ValueTask SetState(this BotRequestContext context, State state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(state);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        return stateMachine.SetState(context.ChatId, state, cancellationToken);
    }*/

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <param name="state"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TState"></typeparam>
    /// <returns></returns>
    public static ValueTask SetState<TState>(this BotRequestContext context, TState state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(state);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        return stateMachine.SetState(context.ChatId, state, cancellationToken);
    }

    /// <summary>
    ///     Deletes the state of the current user in the <see cref="BotRequestContext"/>.
    /// </summary>
    /// <param name="context">The current instance of <see cref="BotRequestContext"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public static ValueTask DropState(this BotRequestContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        return stateMachine.DropState(context.ChatId, cancellationToken);
    }
}
