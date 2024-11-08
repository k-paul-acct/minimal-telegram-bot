using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace MinimalTelegramBot.Pipeline;

/// <summary>
///     UseExtensions.
/// </summary>
public static class UseExtensions
{
    /// <summary>
    ///     Adds a pipe (middleware) to the <see cref="BotApplication"/> pipeline.
    /// </summary>
    /// <param name="app">The bot application builder.</param>
    /// <param name="pipe">The pipe (middleware) to add.</param>
    /// <returns>The bot application builder.</returns>
    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, Func<Task>, Task> pipe)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(pipe);

        return app.Use(next => context => pipe(context, () => next(context)));
    }

    /// <summary>
    ///     Adds a pipe (middleware) to the <see cref="BotApplication"/> pipeline.
    /// </summary>
    /// <param name="app">The bot application builder.</param>
    /// <param name="pipe">The pipe (middleware) to add.</param>
    /// <returns>The bot application builder with added pipe (middleware).</returns>
    public static IBotApplicationBuilder Use(this IBotApplicationBuilder app, Func<BotRequestContext, BotRequestDelegate, Task> pipe)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(pipe);

        return app.Use(next => context => pipe(context, next));
    }

    /// <summary>
    ///     Adds a pipe (middleware) to the <see cref="BotApplication"/> pipeline.
    /// </summary>
    /// <param name="app">The bot application builder.</param>
    /// <param name="pipe">The pipe (middleware) to add.</param>
    /// <returns>The bot application builder with added pipe (middleware).</returns>
    public static IBotApplicationBuilder UsePipe(this IBotApplicationBuilder app, IPipe pipe)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(pipe);

        return app.Use(next => context => pipe.InvokeAsync(context, next));
    }

    /// <summary>
    ///     Adds a pipe (middleware) of type <typeparamref name="TPipe"/> to the <see cref="BotApplication"/> pipeline.
    /// </summary>
    /// <param name="app">The bot application builder.</param>
    /// <typeparam name="TPipe">The type of the pipe (middleware) to add.</typeparam>
    /// <returns>The bot application builder with added pipe (middleware).</returns>
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
