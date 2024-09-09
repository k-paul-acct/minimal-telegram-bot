namespace MinimalTelegramBot.Handling.Filters;

internal static class FilteredBotRequestDelegateBuilder
{
    public static BotRequestDelegate Build(HandlerDelegate handlerDelegate, BotRequestDelegateFactoryOptions options)
    {
        var factoryContext = new BotRequestFilterFactoryContext
        {
            MethodInfo = handlerDelegate.Method,
            Services = options.Services,
        };

        BotRequestFilterDelegate filteredInvocation = async context =>
        {
            context.BotRequestContext.UpdateHandlingStarted = true;
            var result = await handlerDelegate(context.BotRequestContext);
            return result;
        };

        var initialFilteredInvocation = filteredInvocation;

        for (var i = options.HandlerBuilder.FilterFactories.Count - 1; i >= 0; --i)
        {
            var currentFilterFactory = options.HandlerBuilder.FilterFactories[i];
            filteredInvocation = currentFilterFactory(factoryContext, filteredInvocation);
        }

        if (ReferenceEquals(initialFilteredInvocation, filteredInvocation))
        {
            return async context =>
            {
                context.UpdateHandlingStarted = true;
                var result = await handlerDelegate(context);
                await result.ExecuteAsync(context);
            };
        }

        return async context =>
        {
            var result = await filteredInvocation(new BotRequestFilterContext(context));
            await result.ExecuteAsync(context);
        };
    }
}
