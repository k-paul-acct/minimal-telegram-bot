using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MinimalTelegramBot.Handling.Requirements;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling.Filters;

public static class FilterExtensions
{
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

    public static TBuilder Filter<TBuilder>(this TBuilder builder, Func<BotRequestFilterContext, IResult> filterDelegate)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filterDelegate);

        builder.Filter((context, _) =>
        {
            var result = filterDelegate(context);
            return new ValueTask<IResult>(result);
        });

        return builder;
    }

    public static TBuilder Filter<TBuilder>(this TBuilder builder, Func<BotRequestFilterContext, ValueTask<IResult>> filterDelegate)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filterDelegate);

        builder.Filter((context, _) => filterDelegate(context));

        return builder;
    }

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

    public static TBuilder FilterInlineQuery<TBuilder>(this TBuilder builder, Func<string, IResult> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        builder.Filter((context, _) =>
        {
            if (context.BotRequestContext.Update.InlineQuery?.Query is null)
            {
                return new ValueTask<IResult>(Results.Results.Empty);
            }

            var result = filter(context.BotRequestContext.Update.InlineQuery.Query);
            return new ValueTask<IResult>(result);
        });

        var metadata = new UpdateTypeRequirement(UpdateType.InlineQuery);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    public static TBuilder FilterInlineQuery<TBuilder>(this TBuilder builder, Func<string, ValueTask<IResult>> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        builder.Filter((context, _) => context.BotRequestContext.Update.InlineQuery?.Query is null
            ? new ValueTask<IResult>(Results.Results.Empty)
            : filter(context.BotRequestContext.Update.InlineQuery.Query));

        var metadata = new UpdateTypeRequirement(UpdateType.InlineQuery);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }
}
