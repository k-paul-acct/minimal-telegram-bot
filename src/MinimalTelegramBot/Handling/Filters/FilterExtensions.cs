using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MinimalTelegramBot.Handling.Requirements;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling.Filters;

public static class FilterExtensions
{
    /// <summary>
    ///     Adds a filter of type <typeparamref name="TFilter"/> to the handler or handler group.
    /// </summary>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <typeparam name="TFilter">The type of the <see cref="IHandlerFilter"/> to add.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder Filter<TBuilder, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilter>(this TBuilder builder)
        where TFilter : IHandlerFilter
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);

        ObjectFactory filterFactory;

        try
        {
            filterFactory = ActivatorUtilities.CreateFactory(typeof(TFilter), [typeof(BotRequestFilterFactoryContext),]);
        }
        catch (InvalidOperationException)
        {
            filterFactory = ActivatorUtilities.CreateFactory(typeof(TFilter), []);
        }

        builder.AddFilterFactory((factoryContext, next) =>
        {
            return filterContext =>
            {
                var filter = (IHandlerFilter)filterFactory(filterContext.Services, [factoryContext,]);
                return filter.InvokeAsync(filterContext, next);
            };
        });

        return builder;
    }

    /// <summary>
    ///     Adds a filter of type <typeparamref name="TFilter"/> to the handler or handler group.
    /// </summary>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="configure">The action to configure the <see cref="BotRequestFilterContext"/> before filter invocation.</param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <typeparam name="TFilter">The type of the <see cref="IHandlerFilter"/> to add.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder Filter<TBuilder, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilter>(this TBuilder builder, Action<BotRequestFilterContext> configure)
        where TFilter : IHandlerFilter
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        ObjectFactory filterFactory;

        try
        {
            filterFactory = ActivatorUtilities.CreateFactory(typeof(TFilter), [typeof(BotRequestFilterFactoryContext),]);
        }
        catch (InvalidOperationException)
        {
            filterFactory = ActivatorUtilities.CreateFactory(typeof(TFilter), []);
        }

        builder.AddFilterFactory((factoryContext, next) =>
        {
            return filterContext =>
            {
                var filter = (IHandlerFilter)filterFactory(filterContext.Services, [factoryContext,]);
                configure(filterContext);
                return filter.InvokeAsync(filterContext, next);
            };
        });

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group using a custom filter delegate.
    /// </summary>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="filter">The delegate that defines the filter logic.</param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder Filter<TBuilder>(this TBuilder builder, Func<BotRequestFilterContext, BotRequestFilterDelegate, ValueTask<IResult>> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        builder.AddFilterFactory((_, next) => filterContext => filter(filterContext, next));

        return builder;
    }

    private static void AddFilterFactory<TBuilder>(this TBuilder builder, Func<BotRequestFilterFactoryContext, BotRequestFilterDelegate, BotRequestFilterDelegate> factory)
        where TBuilder : IHandlerConventionBuilder
    {
        builder.Add(handlerBuilder => handlerBuilder.FilterFactories.Add(factory));
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group using a custom filter delegate.
    ///     The next filter after the current one will be invoked if the current one returns true,
    ///     otherwise the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="filterDelegate">The delegate that defines the filter logic.</param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder Filter<TBuilder>(this TBuilder builder, Func<BotRequestFilterContext, bool> filterDelegate)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filterDelegate);

        builder.Filter((context, next) => filterDelegate(context)
            ? next(context)
            : new ValueTask<IResult>(Results.Results.Empty));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group using a custom async filter delegate.
    ///     The next filter after the current one will be invoked if the current one returns true,
    ///     otherwise the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="filterDelegate">The async delegate that defines the filter logic.</param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder Filter<TBuilder>(this TBuilder builder, Func<BotRequestFilterContext, ValueTask<bool>> filterDelegate)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filterDelegate);

        builder.Filter(async (context, next) => await filterDelegate(context)
            ? await next(context)
            : Results.Results.Empty);

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the <see cref="Message.Text"/> of an incoming <see cref="Message"/> update matches the specified one.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <remarks>
    ///     This filter implicitly implies that the <see cref="Update.Type"/> of an incoming <see cref="Update"/>
    ///     should be a <see cref="UpdateType.Message"/>.
    /// </remarks>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="messageText">
    ///     Required value of the <see cref="Message.Text"/> of an incoming <see cref="Message"/> update.
    /// </param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterMessageText<TBuilder>(this TBuilder builder, string messageText)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(messageText);

        builder.Filter((context, next) => messageText.Equals(context.BotRequestContext.MessageText, StringComparison.Ordinal)
            ? next(context)
            : new ValueTask<IResult>(Results.Results.Empty));

        var metadata = new UpdateTypeRequirement(UpdateType.Message);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the <see cref="Message.Text"/> of an incoming <see cref="Message"/> update satisfies
    ///     the specified filter.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <remarks>
    ///     This filter implicitly implies that the <see cref="Update.Type"/> of an incoming <see cref="Update"/>
    ///     should be a <see cref="UpdateType.Message"/>.
    /// </remarks>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="filter">
    ///     The predicate to check the <see cref="Message.Text"/> of an incoming <see cref="Message"/> update.
    /// </param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterMessageText<TBuilder>(this TBuilder builder, Func<string, bool> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        builder.Filter((context, next) =>
        {
            if (context.BotRequestContext.MessageText is null)
            {
                return new ValueTask<IResult>(Results.Results.Empty);
            }

            return filter(context.BotRequestContext.MessageText) ? next(context) : new ValueTask<IResult>(Results.Results.Empty);
        });

        var metadata = new UpdateTypeRequirement(UpdateType.Message);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the <see cref="Message.Text"/> of an incoming <see cref="Message"/> update is a command
    ///     and that command matches the specified one, for example <c>/start</c>.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Additional command arguments, such as <c>123</c> in <c>/start 123</c> command, are not counted.
    ///     </para>
    ///     <para>
    ///         This filter implicitly implies that the <see cref="Update.Type"/> of an incoming <see cref="Update"/>
    ///         should be a <see cref="UpdateType.Message"/>.
    ///     </para>
    /// </remarks>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="command">The command.</param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterCommand<TBuilder>(this TBuilder builder, string command)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(command);

        builder.Filter((context, next) =>
        {
            if (context.BotRequestContext.MessageText is null)
            {
                return new ValueTask<IResult>(Results.Results.Empty);
            }

            var span = context.BotRequestContext.MessageText.AsSpan();

            if (span.Length < 2 || span[0] != '/')
            {
                return new ValueTask<IResult>(Results.Results.Empty);
            }

            var commandEnd = 1;

            foreach (var letter in span[1..])
            {
                var uLetter = (uint)letter;

                if (uLetter - 'a' > 'z' - 'a' && uLetter - '0' > '9' - '0' && uLetter != '_')
                {
                    break;
                }

                commandEnd += 1;
            }

            return span[..commandEnd].SequenceEqual(command) ? next(context) : new ValueTask<IResult>(Results.Results.Empty);
        });

        var metadata = new UpdateTypeRequirement(UpdateType.Message);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the <see cref="CallbackQuery.Data"/> of an incoming <see cref="CallbackQuery"/> update matches
    ///     the specified one.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <remarks>
    ///     This filter implicitly implies that the <see cref="Update.Type"/> of an incoming <see cref="Update"/>
    ///     should be a <see cref="UpdateType.CallbackQuery"/>.
    /// </remarks>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="callbackData">
    ///     Required value of the <see cref="CallbackQuery.Data"/> of an incoming <see cref="CallbackQuery"/> update.
    /// </param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterCallbackData<TBuilder>(this TBuilder builder, string callbackData)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(callbackData);

        builder.Filter((context, next) => callbackData.Equals(context.BotRequestContext.CallbackData, StringComparison.Ordinal)
            ? next(context)
            : new ValueTask<IResult>(Results.Results.Empty));

        var metadata = new UpdateTypeRequirement(UpdateType.CallbackQuery);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the <see cref="CallbackQuery.Data"/> of an incoming <see cref="CallbackQuery"/> update satisfies
    ///     the specified filter.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <remarks>
    ///     This filter implicitly implies that the <see cref="Update.Type"/> of an incoming <see cref="Update"/>
    ///     should be a <see cref="UpdateType.CallbackQuery"/>.
    /// </remarks>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="filter">
    ///     The predicate to check the <see cref="CallbackQuery.Data"/> of
    ///     an incoming <see cref="CallbackQuery"/> update.
    /// </param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterCallbackData<TBuilder>(this TBuilder builder, Func<string, bool> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        builder.Filter((context, next) =>
        {
            if (context.BotRequestContext.CallbackData is null)
            {
                return new ValueTask<IResult>(Results.Results.Empty);
            }

            return filter(context.BotRequestContext.CallbackData) ? next(context) : new ValueTask<IResult>(Results.Results.Empty);
        });

        var metadata = new UpdateTypeRequirement(UpdateType.CallbackQuery);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if its <see cref="Update.Type"/> matches the specified one.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="updateType">
    ///     Required value of the <see cref="Update.Type"/> of an incoming <see cref="Update"/>.
    /// </param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterUpdateType<TBuilder>(this TBuilder builder, UpdateType updateType)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Filter((context, next) => context.BotRequestContext.Update.Type == updateType
            ? next(context)
            : new ValueTask<IResult>(Results.Results.Empty));

        var metadata = new UpdateTypeRequirement(updateType);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if its <see cref="Update.Type"/> satisfies the specified filter.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="filter">
    ///     The predicate to check the <see cref="Update.Type"/> of an incoming <see cref="Update"/>.
    /// </param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterUpdateType<TBuilder>(this TBuilder builder, Func<UpdateType, bool> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        builder.Filter((context, next) => filter(context.BotRequestContext.Update.Type)
            ? next(context)
            : new ValueTask<IResult>(Results.Results.Empty));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the <see cref="InlineQuery.Query"/> of an incoming <see cref="InlineQuery"/> update matches
    ///     the specified one.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <remarks>
    ///     This filter implicitly implies that the <see cref="Update.Type"/> of an incoming <see cref="Update"/>
    ///     should be an <see cref="UpdateType.InlineQuery"/>.
    /// </remarks>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="query">
    ///     Required value of the <see cref="InlineQuery.Query"/> of an incoming <see cref="InlineQuery"/>.
    /// </param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterInlineQuery<TBuilder>(this TBuilder builder, string query)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(query);

        builder.Filter((context, next) => query.Equals(context.BotRequestContext.Update.InlineQuery?.Query, StringComparison.Ordinal)
            ? next(context)
            : new ValueTask<IResult>(Results.Results.Empty));

        var metadata = new UpdateTypeRequirement(UpdateType.InlineQuery);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the <see cref="InlineQuery.Query"/> of an incoming <see cref="InlineQuery"/> update satisfies
    ///     the specified filter.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <remarks>
    ///     This filter implicitly implies that the <see cref="Update.Type"/> of an incoming <see cref="Update"/>
    ///     should be an <see cref="UpdateType.InlineQuery"/>.
    /// </remarks>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="filter">
    ///     The predicate to check the <see cref="InlineQuery.Query"/> of an incoming <see cref="InlineQuery"/> update.
    /// </param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterInlineQuery<TBuilder>(this TBuilder builder, Func<string, bool> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        builder.Filter((context, next) =>
        {
            if (context.BotRequestContext.Update.InlineQuery?.Query is null)
            {
                return new ValueTask<IResult>(Results.Results.Empty);
            }

            return filter(context.BotRequestContext.Update.InlineQuery.Query)
                ? next(context)
                : new ValueTask<IResult>(Results.Results.Empty);
        });

        var metadata = new UpdateTypeRequirement(UpdateType.InlineQuery);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    /// <summary>
    ///     Adds a filter to the handler or handler group that allows an incoming <see cref="Update"/> to pass further down
    ///     the filter pipeline if the <see cref="InlineQuery.Query"/> of an incoming <see cref="InlineQuery"/> update satisfies
    ///     the specified async filter.
    ///     If not, the <see cref="Results.Empty"/> result will be used as the result of the bot request.
    /// </summary>
    /// <remarks>
    ///     This filter implicitly implies that the <see cref="Update.Type"/> of an incoming <see cref="Update"/>
    ///     should be an <see cref="UpdateType.InlineQuery"/>.
    /// </remarks>
    /// <param name="builder">The handler or handler group that implements <see cref="IHandlerConventionBuilder"/>.</param>
    /// <param name="filter">
    ///     The async predicate to check the <see cref="InlineQuery.Query"/> of an incoming <see cref="InlineQuery"/> update.
    /// </param>
    /// <typeparam name="TBuilder">The type of <see cref="IHandlerConventionBuilder"/> to configure.</typeparam>
    /// <returns>A <typeparamref name="TBuilder"/> that can be used to further customize the handler or handler group.</returns>
    public static TBuilder FilterInlineQuery<TBuilder>(this TBuilder builder, Func<string, ValueTask<bool>> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        builder.Filter(async (context, next) =>
        {
            if (context.BotRequestContext.Update.InlineQuery?.Query is null)
            {
                return Results.Results.Empty;
            }

            return await filter(context.BotRequestContext.Update.InlineQuery.Query)
                ? await next(context)
                : Results.Results.Empty;
        });

        var metadata = new UpdateTypeRequirement(UpdateType.InlineQuery);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }
}
