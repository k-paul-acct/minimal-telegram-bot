namespace MinimalTelegramBot.Pipeline;

public static class UseExtensions
{
    public static IBotApplicationBuilder UseCallbackAutoAnswering(this IBotApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.Properties["__CallbackAutoAnsweringAdded"] = true;
        return app;
    }

    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, Func<Task>, Task> pipe)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(pipe);

        return app.Use(next => context => pipe(context, () => next(context)));
    }

    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, Func<BotRequestContext, Task>, Task> pipe)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(pipe);

        return app.Use(next => context => pipe(context, next));
    }

    public static IBotApplicationBuilder UsePipe<TPipe>(this IBotApplicationBuilder app) where TPipe : IPipe
    {
        ArgumentNullException.ThrowIfNull(app);

        return app.UsePipe(typeof(TPipe));
    }

    private static IBotApplicationBuilder UsePipe(this IBotApplicationBuilder app, Type pipeType)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(pipeType);

        return app.Use((ctx, next) =>
        {
            var pipeService = (IPipe)ctx.Services.GetRequiredService(pipeType);
            return pipeService.InvokeAsync(ctx, next);
        });
    }
}