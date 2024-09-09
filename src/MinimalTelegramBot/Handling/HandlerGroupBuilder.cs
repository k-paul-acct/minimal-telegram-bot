namespace MinimalTelegramBot.Handling;

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
    ICollection<HandlerSource> IHandlerDispatcher.HandlerSources { get; } = new List<HandlerSource>();

    private sealed class HandlerGroupHandlerSource : HandlerSource
    {
        private readonly HandlerGroupBuilder _groupBuilder;

        public HandlerGroupHandlerSource(HandlerGroupBuilder groupBuilder)
        {
            _groupBuilder = groupBuilder;
        }

        public override IReadOnlyCollection<Handler> Handlers => []; // GetHandlers();

        /*private IReadOnlyList<Handler> GetHandlers()
        {
            var handlers = new List<Handler>();

            _groupBuilder._handlerSources

            handlers.AddRange(_groupBuilder._handlerSources);
        }*/
    }
}
