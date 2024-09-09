namespace MinimalTelegramBot.Handling;

public sealed class HandlerResolver
{
    private readonly List<Handler> _handlers;

    public HandlerResolver(ICollection<HandlerSource> handlerSources)
    {
        _handlers = [];

        foreach (var handlerSource in handlerSources)
        {
            _handlers.AddRange(handlerSource.Handlers);
        }
    }

    public Func<BotRequestContext, BotRequestDelegate, Task> BuildFullPipeline()
    {
        return async (context, _) =>
        {
            var updateType = context.Update.Type;
            var updateTypeRequirement = new UpdateHandlingRequirement(new UpdateTypeAttribute(updateType));
            UpdateHandlingRequirement[] requirements = [updateTypeRequirement,];

            foreach (var handler in _handlers)
            {
                var satisfy = handler.SatisfyRequirements(requirements);
                if (!satisfy)
                {
                    continue;
                }

                await handler.RequestDelegate(context);

                if (context.UpdateHandlingStarted)
                {
                    break;
                }
            }
        };
    }
}
