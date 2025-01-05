using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Pipeline;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Extensions;

/// <summary>
///     BotApplicationBuilderExtensions.
/// </summary>
public static class BotApplicationBuilderExtensions
{
    /// <summary>
    ///     Adds state machine middleware to the bot application builder.
    /// </summary>
    /// <param name="app">The bot application builder.</param>
    /// <returns>The current instance of <see cref="IBotApplicationBuilder"/>.</returns>
    public static IBotApplicationBuilder UseStateMachine(this IBotApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.Use(async (context, next) =>
        {
            var stateMachine = context.Services.GetRequiredService<IStateMachine>();
            var options = context.Services.GetRequiredService<IOptions<StateManagementOptions>>().Value;
            var stateEntryContext = context.Update.CreateStateEntryContext(options.TrackingStrategy);
            var state = await stateMachine.GetState<object>(stateEntryContext);
            context.Data["__State"] = state;
            await next(context);
        });
    }
}
