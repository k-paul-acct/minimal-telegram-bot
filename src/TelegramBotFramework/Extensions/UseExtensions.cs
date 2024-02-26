using TelegramBotFramework.Pipeline;

namespace TelegramBotFramework.Extensions;

public static class UseExtensions
{
    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, Func<Task>, Task> pipe)
    {
        return app.Use(next => context => pipe(context, () => next(context)));
    }

    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, BotRequestDelegate, Task> pipe)
    {
        return app.Use(next => context => pipe(context, next));
    }
}