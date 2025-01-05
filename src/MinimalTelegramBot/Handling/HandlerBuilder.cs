using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MinimalTelegramBot.Handling;

/// <summary>
///     A builder for a single <see cref="Handler"/> that processes bot request.
/// </summary>
public sealed class HandlerBuilder : IHandlerConventionBuilder
{
    private readonly List<Action<HandlerBuilder>> _conventions;
    private readonly Delegate _handler;
    private readonly IHandlerDispatcher _handlerDispatcher;

    /// <summary>
    ///     Initializes a new instance of the <see cref="HandlerBuilder"/>.
    /// </summary>
    /// <param name="handlerDispatcher">The dispatcher responsible for handling the bot requests.</param>
    /// <param name="handler">The delegate that represents the handler method.</param>
    public HandlerBuilder(IHandlerDispatcher handlerDispatcher, Delegate handler)
    {
        _handlerDispatcher = handlerDispatcher;
        _handler = handler;
        _conventions = [];
        Metadata = [];
        FilterFactories = [];

        _handlerDispatcher.HandlerSources.Add(new SingleHandlerHandlerSource(this));
    }

    /// <summary>
    ///    Gets the metadata associated with the handler.
    /// </summary>
    public List<object> Metadata { get; }

    /// <summary>
    ///     Gets the filter factories that will be used to create the filtering pipeline for the handler.
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

        public IReadOnlyCollection<Handler> GetHandlers(IReadOnlyCollection<Action<HandlerBuilder>> conventions)
        {
            IEnumerable<Action<HandlerBuilder>> fullConventions = [..conventions, .._handlerBuilder._conventions,];

            foreach (var convention in fullConventions)
            {
                convention(_handlerBuilder);
            }

            var metadata = ConstructHandlerMetadata();
            var builderOptions = _handlerBuilder._handlerDispatcher.Services.GetRequiredService<IOptions<HandlerDelegateBuilderOptions>>();
            var handlerDelegate = HandlerDelegateBuilder.Build(_handlerBuilder._handler, builderOptions.Value.Interceptors);
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
