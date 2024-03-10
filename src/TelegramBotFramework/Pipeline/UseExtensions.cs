namespace TelegramBotFramework.Pipeline;

public static class UseExtensions
{
    public static IBotApplicationBuilder UseCallbackAutoAnswering(this IBotApplicationBuilder app)
    {
        app.Properties["CallbackAutoAnsweringAdded"] = true;
        return app;
    }

    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, Func<Task>, Task> pipe)
    {
        return app.Use(next => context => pipe(context, () => next(context)));
    }

    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, BotRequestDelegate, Task> pipe)
    {
        return app.Use(next => context => pipe(context, next));
    }
}