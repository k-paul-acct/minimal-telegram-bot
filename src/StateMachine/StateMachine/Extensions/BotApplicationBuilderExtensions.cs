using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Pipeline;
using MinimalTelegramBot.StateMachine.Pipes;

namespace MinimalTelegramBot.StateMachine.Extensions;

public static class BotApplicationBuilderExtensions
{
    public static IBotApplicationBuilder UseStateMachine(this IBotApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.UsePipe<StateMachinePipe>();
        return app;
    }
}
