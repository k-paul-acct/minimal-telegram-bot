using IResult = MinimalTelegramBot.Results.IResult;

namespace MinimalTelegramBot.Handling;

public class Handler : IHandlerFilter
{
    private readonly List<Func<BotRequestContext, bool>> _filterDelegates = [];
    private readonly Func<BotRequestContext, Task<IResult>> _handlerDelegate;

    public Handler(Delegate handlerDelegate)
    {
        _handlerDelegate = HandlerDelegateBuilder.Build(handlerDelegate);
    }

    public Handler Filter(Func<BotRequestContext, bool> filterDelegate)
    {
        _filterDelegates.Add(filterDelegate);
        return this;
    }

    public bool CanHandle(BotRequestContext context)
    {
        return _filterDelegates.Count == 0 || _filterDelegates.All(x => x(context));
    }

    internal async Task Handle(BotRequestContext context)
    {
        var result = await _handlerDelegate(context);
        await result.ExecuteAsync(context);
    }
}