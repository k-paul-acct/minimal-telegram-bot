using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling.Filters;

public static class FilterExtensions
{
    public static TBuilder Filter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilter, TBuilder>(this TBuilder builder)
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
                var filter = (IHandlerFilter)filterFactory(factoryContext.Services, [factoryContext,]);
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

    public static TBuilder Filter<TBuilder>(this TBuilder builder, Func<BotRequestFilterContext, IResult> filterDelegate)
        where TBuilder : IHandlerConventionBuilder
    {
        throw new NotImplementedException();
    }

    public static TBuilder Filter<TBuilder>(this TBuilder builder, Func<BotRequestFilterContext, ValueTask<IResult>> filterDelegate)
        where TBuilder : IHandlerConventionBuilder
    {
        throw new NotImplementedException();
    }

    public static TBuilder Filter<TBuilder>(this TBuilder builder, Func<BotRequestFilterContext, bool> filterDelegate)
        where TBuilder : IHandlerConventionBuilder
    {
        throw new NotImplementedException();
    }

    public static TBuilder Filter<TBuilder>(this TBuilder builder, Func<BotRequestFilterContext, ValueTask<bool>> filterDelegate)
        where TBuilder : IHandlerConventionBuilder
    {
        throw new NotImplementedException();
    }

    private static TBuilder AddFilterFactory<TBuilder>(this TBuilder builder, Func<BotRequestFilterFactoryContext, BotRequestFilterDelegate, BotRequestFilterDelegate> factory)
        where TBuilder : IHandlerConventionBuilder
    {
        builder.Add(handlerBuilder => handlerBuilder.FilterFactories.Add(factory));
        return builder;
    }

    public static TBuilder FilterText<TBuilder>(this TBuilder builder, Func<string, bool> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.Filter(ctx => ctx.BotRequestContext.MessageText is not null && filter(ctx.BotRequestContext.MessageText));
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

        var metadata = new UpdateTypeAttribute(UpdateType.Message);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    public static TBuilder FilterCallbackData<TBuilder>(this TBuilder builder, Func<string, bool> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.Filter(ctx => ctx.BotRequestContext.CallbackData is not null && filter(ctx.BotRequestContext.CallbackData));
    }

    public static TBuilder FilterUpdateType<TBuilder>(this TBuilder builder, UpdateType updateType)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Filter((context, next) => context.BotRequestContext.Update.Type == updateType
            ? next(context)
            : new ValueTask<IResult>(Results.Results.Empty));

        var metadata = new UpdateTypeAttribute(updateType);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    public static TBuilder FilterUpdateType<TBuilder>(this TBuilder builder, Func<UpdateType, bool> filter)
        where TBuilder : IHandlerConventionBuilder
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.Filter(ctx => filter(ctx.BotRequestContext.Update.Type));
    }
}
