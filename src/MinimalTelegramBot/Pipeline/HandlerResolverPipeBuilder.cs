using MinimalTelegramBot.Handling.Requirements;
using Telegram.Bot.Types.Enums;

namespace MinimalTelegramBot.Pipeline;

internal static class HandlerResolverPipeBuilder
{
    public static Func<BotRequestDelegate, BotRequestDelegate> Build(ICollection<IHandlerSource> handlerSources)
    {
        List<Handler> handlers = [];

        foreach (var handlerSource in handlerSources)
        {
            handlers.AddRange(handlerSource.GetHandlers([]));
        }

        var maxIndex = (int)Enum.GetValues<UpdateType>().Max();
        var n = maxIndex + 1;
        var handlersByUpdateType = new List<Handler>[n];

        for (var i = 0; i < n; ++i)
        {
            handlersByUpdateType[i] = [];
        }

        foreach (var handler in handlers)
        {
            handler.Metadata.Remove(typeof(UpdateTypeRequirement), out var requirements);
            requirements ??= [];

            var updateTypeHashSet = new HashSet<uint>();

            foreach (var requirement in requirements)
            {
                var type = (uint)((UpdateTypeRequirement)requirement).UpdateType;
                updateTypeHashSet.Add(type);
            }

            if (updateTypeHashSet.Count > 1)
            {
                continue;
            }

            var updateType = updateTypeHashSet.FirstOrDefault();
            updateType = updateType > maxIndex ? 0 : updateType;

            handlersByUpdateType[updateType].Add(handler);
        }

        for (var i = 1; i < n; ++i)
        {
            handlersByUpdateType[i].AddRange(handlersByUpdateType[0]);
        }

        var handlersByUpdateTypeCache = handlersByUpdateType.Select(x => x.ToArray()).ToArray();

        return _ => async context =>
        {
            var updateType = (int)context.Update.Type;
            var availableHandlers = handlersByUpdateTypeCache[updateType];

            foreach (var handler in availableHandlers)
            {
                var satisfy = handler.SatisfyRequirements([]);
                if (!satisfy)
                {
                    continue;
                }

                await handler.FilteredBotRequestDelegate(context);

                if (context.Data.ContainsKey("__UpdateHandlingStarted"))
                {
                    return;
                }
            }
        };
    }
}
