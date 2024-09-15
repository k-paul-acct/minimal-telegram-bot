using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalTelegramBot.Pipeline;

public static class UseExtensions
{
    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, Func<Task>, Task> pipe)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(pipe);

        return app.Use(next => context => pipe(context, () => next(context)));
    }

    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, BotRequestDelegate, Task> pipe)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(pipe);

        return app.Use(next => context => pipe(context, next));
    }

    public static IBotApplicationBuilder UsePipe(this IBotApplicationBuilder app, IPipe pipe)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(pipe);

        return app.Use(next => context => pipe.InvokeAsync(context, next));
    }

    public static IBotApplicationBuilder UsePipe<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TPipe>(this IBotApplicationBuilder app)
        where TPipe : IPipe
    {
        ArgumentNullException.ThrowIfNull(app);
        return app.UsePipe(typeof(TPipe));
    }

    private static IBotApplicationBuilder UsePipe(this IBotApplicationBuilder app, Type pipeType)
    {
        return app.Use(next => context =>
        {
            var instance = (IPipe)ActivatorUtilities.CreateInstance(context.Services, pipeType);
            return instance.InvokeAsync(context, next);
        });
    }
}
