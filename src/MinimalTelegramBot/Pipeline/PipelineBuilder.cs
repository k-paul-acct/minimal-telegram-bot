namespace MinimalTelegramBot.Pipeline;

internal sealed class PipelineBuilder : IBotApplicationBuilder
{
    private readonly List<Func<BotRequestDelegate, BotRequestDelegate>> _pipes = [];

    public PipelineBuilder(IServiceProvider services, IDictionary<string, object?> properties)
    {
        Services = services;
        Properties = properties;
    }

    public IServiceProvider Services { get; }
    public IDictionary<string, object?> Properties { get; }

    public IBotApplicationBuilder Use(Func<BotRequestDelegate, BotRequestDelegate> pipe)
    {
        _pipes.Add(pipe);
        return this;
    }

    public BotRequestDelegate Build()
    {
        BotRequestDelegate pipeline = _ => Task.CompletedTask;

        for (var i = _pipes.Count - 1; i >= 0; --i)
        {
            pipeline = _pipes[i](pipeline);
        }

        return pipeline;
    }
}
