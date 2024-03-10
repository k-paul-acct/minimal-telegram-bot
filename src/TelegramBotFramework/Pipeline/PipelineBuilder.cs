namespace TelegramBotFramework.Pipeline;

internal class PipelineBuilder : IBotApplicationBuilder
{
    public const string RequestUnhandledKey = "RequestUnhandled";

    private readonly List<Func<BotRequestDelegate, BotRequestDelegate>> _pipes = [];

    public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>();

    public IBotApplicationBuilder Use(Func<BotRequestDelegate, BotRequestDelegate> pipe)
    {
        _pipes.Add(pipe);
        return this;
    }

    public BotRequestDelegate Build()
    {
        BotRequestDelegate pipeline = context =>
        {
            if (!context.UpdateHandlingStarted)
            {
                context.Data[RequestUnhandledKey] = true;
            }

            return Task.CompletedTask;
        };

        for (var i = _pipes.Count - 1; i >= 0; --i)
        {
            pipeline = _pipes[i](pipeline);
        }

        return pipeline;
    }
}