namespace MinimalTelegramBot.Handling;

/// <summary>
///     A builder for a group of handlers.
/// </summary>
public sealed class HandlerGroupBuilder : IHandlerDispatcher, IHandlerConventionBuilder
{
    private readonly IHandlerDispatcher _outerDispatcher;
    private readonly List<Action<HandlerBuilder>> _conventions;

    internal HandlerGroupBuilder(IHandlerDispatcher outerDispatcher)
    {
        _outerDispatcher = outerDispatcher;
        _conventions = [];

        _outerDispatcher.HandlerSources.Add(new HandlerGroupHandlerSource(this));
    }

    void IHandlerConventionBuilder.Add(Action<HandlerBuilder> convention)
    {
        _conventions.Add(convention);
    }

    IServiceProvider IHandlerDispatcher.Services => _outerDispatcher.Services;
    ICollection<IHandlerSource> IHandlerDispatcher.HandlerSources { get; } = new List<IHandlerSource>();

    private sealed class HandlerGroupHandlerSource : IHandlerSource
    {
        private readonly HandlerGroupBuilder _groupBuilder;

        public HandlerGroupHandlerSource(HandlerGroupBuilder groupBuilder)
        {
            _groupBuilder = groupBuilder;
        }

        public IReadOnlyList<Handler> GetHandlers(IReadOnlyList<Action<HandlerBuilder>> conventions)
        {
            var handlers = new List<Handler>();
            IHandlerDispatcher handlerDispatcher = _groupBuilder;
            IReadOnlyList<Action<HandlerBuilder>> sourceOuterConventions = [..conventions, .._groupBuilder._conventions];

            foreach (var handlerSource in handlerDispatcher.HandlerSources)
            {
                var sourceHandlers = handlerSource.GetHandlers(sourceOuterConventions);
                handlers.AddRange(sourceHandlers);
            }

            return handlers;
        }
    }
}
