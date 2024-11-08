namespace MinimalTelegramBot.Handling;

/// <summary>
///     A builder for a single <see cref="Handler"/> that processes bot request.
/// </summary>
public sealed class HandlerBuilder : IHandlerConventionBuilder
{
    private readonly List<Action<HandlerBuilder>> _conventions;
    private readonly IHandlerDispatcher _handlerDispatcher;
    private readonly Delegate _handler;

    /// <summary>
    /// </summary>
    /// <param name="handlerDispatcher"></param>
    /// <param name="handler"></param>
    /// <param name="botRequestDelegate"></param>
    public HandlerBuilder(IHandlerDispatcher handlerDispatcher, Delegate handler, BotRequestDelegate? botRequestDelegate)
    {
        _handlerDispatcher = handlerDispatcher;
        _handler = handler;
        _conventions = [];
        Metadata = [];
        BotRequestDelegate = botRequestDelegate;
        FilterFactories = [];

        _handlerDispatcher.HandlerSources.Add(new SingleHandlerHandlerSource(this));
    }

    /// <summary>
    /// </summary>
    public List<object> Metadata { get; }

    /// <summary>
    /// </summary>
    public BotRequestDelegate? BotRequestDelegate { get; set; }

    /// <summary>
    /// </summary>
    public List<Func<BotRequestFilterFactoryContext, BotRequestFilterDelegate, BotRequestFilterDelegate>> FilterFactories { get; }

    void IHandlerConventionBuilder.Add(Action<HandlerBuilder> convention)
    {
        _conventions.Add(convention);
    }

    private sealed class SingleHandlerHandlerSource : IHandlerSource
    {
        private readonly HandlerBuilder _handlerBuilder;

        public SingleHandlerHandlerSource(HandlerBuilder handlerBuilder)
        {
            _handlerBuilder = handlerBuilder;
        }

        public IReadOnlyList<Handler> GetHandlers(IReadOnlyList<Action<HandlerBuilder>> conventions)
        {
            IEnumerable<Action<HandlerBuilder>> fullConventions = [..conventions, .._handlerBuilder._conventions];

            foreach (var convention in fullConventions)
            {
                convention(_handlerBuilder);
            }

            var metadata = ConstructHandlerMetadata();
            var handlerDelegate = HandlerDelegateBuilder.Build(_handlerBuilder._handler);
            var options = new BotRequestDelegateFactoryOptions
            {
                Services = _handlerBuilder._handlerDispatcher.Services,
                HandlerBuilder = _handlerBuilder,
            };

            var filtered = FilteredBotRequestDelegateBuilder.Build(handlerDelegate, options);
            var handler = new Handler(filtered, metadata);

            return [handler,];
        }

        private Dictionary<Type, object[]> ConstructHandlerMetadata()
        {
            var metadata = _handlerBuilder.Metadata
                .GroupBy(x => x.GetType())
                .ToDictionary(x => x.Key, x => x.ToArray());

            return metadata;
        }
    }
}
