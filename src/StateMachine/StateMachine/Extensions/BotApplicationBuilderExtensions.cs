using Microsoft.Extensions.DependencyInjection;
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
            var state = await stateMachine.GetState<object>(context.ChatId);
            context.Data["__State"] = state;
            await next(context);
        });
    }
}
