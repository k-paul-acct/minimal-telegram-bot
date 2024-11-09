using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Pipeline;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class BotApplicationBuilderExtensions
{
    public static IBotApplicationBuilder UseStateMachine(this IBotApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.Use(async (context, next) =>
        {
            var stateMachine = context.Services.GetRequiredService<IStateMachine>();
            var options = context.Services.GetRequiredService<IOptions<StateManagementOptions>>().Value;
            var stateEntryContext = context.Update.CreateStateEntryContext(options.StateTrackingStrategy);
            var state = await stateMachine.GetState<object>(stateEntryContext);
            context.Data["__State"] = state;
            await next(context);
        });
    }
}
