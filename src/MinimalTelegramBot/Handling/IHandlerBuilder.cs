namespace MinimalTelegramBot.Handling;

public interface IHandlerBuilder
{
    Handler Handle(Delegate handlerDelegate);
    Handler Handle(Func<BotRequestContext, Task> func);
    ValueTask<Handler?> TryResolveHandler(BotRequestFilterContext ctx);
}

public interface IHandlerBuilder2
{
    void AddConvention(Action<IHandlerBuilder2> convention);
}

public class HandlerBuilder2 : IHandlerBuilder2
{
    private readonly ICollection<Action<IHandlerBuilder2>> _conventions;

    public HandlerBuilder2(ICollection<Action<IHandlerBuilder2>> conventions)
    {
        _conventions = conventions;
    }

    void IHandlerBuilder2.AddConvention(Action<IHandlerBuilder2> convention)
    {
        _conventions.Add(convention);
    }
}

public class Handler2
{
    public readonly Dictionary<object, object?> Metadata;

    // TODO:
    // private readonly List<Func<BotRequestContext, bool>> _filterDelegates = [];

    public readonly Func<BotRequestContext, Task<IResult>> HandlerDelegate;

    public Handler2(Func<BotRequestContext, Task<IResult>> handlerDelegate, Dictionary<object, object?> metadata)
    {
        HandlerDelegate = handlerDelegate;
        Metadata = metadata;
    }

    public async Task Handle(BotRequestContext context)
    {
        var result = await HandlerDelegate(context);
        await result.ExecuteAsync(context);
    }
}

public interface IHandlerDispatcher
{
    IServiceProvider ServiceProvider { get; }
    ICollection<HandlerSource> HandlerSources { get; }
    ICollection<Func<BotRequestContext, Func<BotRequestContext, ValueTask<bool>>>> FilterFactories { get; }
}

public abstract class HandlerSource
{
    public abstract IReadOnlyCollection<Handler2> Handlers { get; }
}

public class HandlerGroupBuilder : IHandlerDispatcher, IHandlerBuilder2
{
    private readonly IHandlerDispatcher _outerDispatcher;
    private readonly List<HandlerSource> _handlerSources = [];
    private readonly List<Action<IHandlerBuilder2>> _conventions = [];
    private readonly List<Func<BotRequestContext, Func<BotRequestContext, ValueTask<bool>>>> _filterFactories = [];

    internal HandlerGroupBuilder(IHandlerDispatcher outerDispatcher)
    {
        _outerDispatcher = outerDispatcher;
        _outerDispatcher.HandlerSources.Add(new HandlerGroupSource(this));
    }

    IServiceProvider IHandlerDispatcher.ServiceProvider => _outerDispatcher.ServiceProvider;
    ICollection<HandlerSource> IHandlerDispatcher.HandlerSources => _outerDispatcher.HandlerSources;
    ICollection<Func<BotRequestContext, Func<BotRequestContext, ValueTask<bool>>>> IHandlerDispatcher.FilterFactories => _filterFactories;

    void IHandlerBuilder2.AddConvention(Action<IHandlerBuilder2> convention)
    {
        _conventions.Add(convention);
    }

    private class HandlerGroupSource : HandlerSource
    {
        private readonly HandlerGroupBuilder _groupBuilder;

        public HandlerGroupSource(HandlerGroupBuilder groupBuilder)
        {
            _groupBuilder = groupBuilder;
        }

        public override IReadOnlyCollection<Handler2> Handlers => GetHandlers();

        private IReadOnlyList<Handler2> GetHandlers()
        {
            var handlers = new List<Handler2>();
            handlers.AddRange(_groupBuilder._handlerSources);
        }
    }
}

public static class HandlerDispatcherExtensions
{
    public static HandlerGroupBuilder HandleGroup(this IHandlerDispatcher handlerDispatcher)
    {
        throw new NotImplementedException();
    }

    public static HandlerBuilder Handle(this IHandlerDispatcher handlerDispatcher, Delegate handler)
    {
        throw new NotImplementedException();
    }

    public static HandlerBuilder Handle(this IHandlerDispatcher handlerDispatcher, Func<BotRequestContext, Task> handler)
    {
        throw new NotImplementedException();
    }
}
