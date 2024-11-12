using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Pipeline;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine.Extensions;

// TODO: Docs.
public static class BotApplicationBuilderExtensions
{
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
