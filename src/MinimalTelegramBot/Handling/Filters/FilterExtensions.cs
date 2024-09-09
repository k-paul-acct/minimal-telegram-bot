using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Handling.Filters;

public static class FilterExtensions
{
    public static IHandlerConventionBuilder Filter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilter>(this IHandlerConventionBuilder builder)
        where TFilter : IHandlerFilter
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

    public static IHandlerConventionBuilder Filter(this IHandlerConventionBuilder builder, Func<BotRequestFilterContext, BotRequestFilterDelegate, ValueTask<IResult>> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        builder.AddFilterFactory((_, next) => filterContext => filter(filterContext, next));

        return builder;
    }

    public static IHandlerConventionBuilder Filter(this IHandlerConventionBuilder builder, Func<BotRequestFilterContext, IResult> filterDelegate)
    {
        throw new NotImplementedException();
    }

    public static IHandlerConventionBuilder Filter(this IHandlerConventionBuilder builder, Func<BotRequestFilterContext, ValueTask<IResult>> filterDelegate)
    {
        throw new NotImplementedException();
    }

    public static IHandlerConventionBuilder Filter(this IHandlerConventionBuilder builder, Func<BotRequestFilterContext, bool> filterDelegate)
    {
        throw new NotImplementedException();
    }

    public static IHandlerConventionBuilder Filter(this IHandlerConventionBuilder builder, Func<BotRequestFilterContext, ValueTask<bool>> filterDelegate)
    {
        throw new NotImplementedException();
    }

    private static IHandlerConventionBuilder AddFilterFactory(this IHandlerConventionBuilder builder, Func<BotRequestFilterFactoryContext, BotRequestFilterDelegate, BotRequestFilterDelegate> factory)
    {
        builder.Add(handlerBuilder => handlerBuilder.FilterFactories.Add(factory));
        return builder;
    }

    public static IHandlerConventionBuilder FilterText(this IHandlerConventionBuilder builder, Func<string, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.Filter(ctx => ctx.BotRequestContext.MessageText is not null && filter(ctx.BotRequestContext.MessageText));
    }

    public static IHandlerConventionBuilder FilterCommand(this IHandlerConventionBuilder builder, string command)
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

    public static IHandlerConventionBuilder FilterCallbackData(this IHandlerConventionBuilder builder, Func<string, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.Filter(ctx => ctx.BotRequestContext.CallbackData is not null && filter(ctx.BotRequestContext.CallbackData));
    }

    public static IHandlerConventionBuilder FilterUpdateType(this IHandlerConventionBuilder builder, UpdateType updateType)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Filter((context, next) => context.BotRequestContext.Update.Type == updateType
            ? next(context)
            : new ValueTask<IResult>(Results.Results.Empty));

        var metadata = new UpdateTypeAttribute(updateType);
        builder.Add(handlerBuilder => handlerBuilder.Metadata.Add(metadata));

        return builder;
    }

    public static IHandlerConventionBuilder FilterUpdateType(this IHandlerConventionBuilder builder, Func<UpdateType, bool> filter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(filter);

        return builder.Filter(ctx => filter(ctx.BotRequestContext.Update.Type));
    }
}
