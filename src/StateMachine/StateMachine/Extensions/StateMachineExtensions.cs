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
    public static ValueTask<State?> GetState(this BotRequestContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        return stateMachine.GetState(context.ChatId, cancellationToken);
    }

    /// <summary>
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

    /// <summary>
    ///     Checks if the user with the specified ID is in the specified state.
    /// </summary>
    /// <param name="context">The current instance of <see cref="BotRequestContext"/>.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="state">State to check against of or null to check if user has no state.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>
    ///     The <see cref="ValueTask"/> that represents the asynchronous operation,
    ///     containing true if the user has state that matches the specified one, false otherwise.
    /// </returns>
    public static async ValueTask<bool> CheckIfInState(this BotRequestContext context, long userId, State? state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        var current = await stateMachine.GetState(userId, cancellationToken);
        return state == current;
    }

    /// <summary>
    ///     Checks if the current user in the <see cref="BotRequestContext"/> is in the specified state.
    /// </summary>
    /// <param name="context">The current instance of <see cref="BotRequestContext"/>.</param>
    /// <param name="state">State to check against of or null to check if user has no state.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>
    ///     The <see cref="ValueTask"/> that represents the asynchronous operation,
    ///     containing true if the user has state that matches the specified one, false otherwise.
    /// </returns>
    public static async ValueTask<bool> CheckIfInState(this BotRequestContext context, State? state, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);
        var stateMachine = context.Services.GetRequiredService<IStateMachine>();
        var current = await stateMachine.GetState(context.ChatId, cancellationToken);
        return state == current;
    }
}
