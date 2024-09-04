using System.Diagnostics.CodeAnalysis;
using MinimalTelegramBot.Handling.Filters;

namespace MinimalTelegramBot.Handling;

public class Handler
{
    private readonly List<(Func<BotRequestFilterContext, ValueTask<bool>> Delegate, IList<object?>? Arguments)> _filterDelegates = [];
    private readonly Func<BotRequestContext, Task<IResult>> _handlerDelegate;

    public Handler(Delegate handlerDelegate)
    {
        ArgumentNullException.ThrowIfNull(handlerDelegate);

        _handlerDelegate = HandlerDelegateBuilder.Build(handlerDelegate);
    }

    public Handler Filter(Func<BotRequestFilterContext, bool> filterDelegate)
    {
        ArgumentNullException.ThrowIfNull(filterDelegate);

        _filterDelegates.Add((context => ValueTask.FromResult(filterDelegate(context)), null));
        return this;
    }

    public Handler Filter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilter>() where TFilter : class, IHandlerFilter
    {
        return FilterWithFactory<TFilter>(null);
    }

    public Handler Filter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilter>(object?[] arguments) where TFilter : class, IHandlerFilter
    {
        return FilterWithFactory<TFilter>(arguments);
    }

    private Handler FilterWithFactory<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFilter>(object?[]? arguments) where TFilter : class, IHandlerFilter
    {
        ObjectFactory filterFactory;

        try
        {
            filterFactory = ActivatorUtilities.CreateFactory(typeof(TFilter), [typeof(BotRequestFilterContext),]);
        }
        catch (InvalidOperationException)
        {
            filterFactory = ActivatorUtilities.CreateFactory(typeof(TFilter), []);
        }

        _filterDelegates.Add((filterContext =>
        {
            var filter = (IHandlerFilter)filterFactory(filterContext.Services, [filterContext,]);
            return filter.Filter(filterContext);
        }, arguments));

        return this;
    }

    internal async ValueTask<bool> CanHandle(BotRequestFilterContext context)
    {
        foreach (var (filterDelegate, arguments) in _filterDelegates)
        {
            if (arguments is not null)
            {
                context.FilterArguments = arguments;
            }

            if (!await filterDelegate(context))
            {
                return false;
            }
        }

        return true;
    }

    internal async Task Handle(BotRequestContext context)
    {
        var result = await _handlerDelegate(context);
        await result.ExecuteAsync(context);
    }
}