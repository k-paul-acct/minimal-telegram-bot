namespace MinimalTelegramBot.Results.Extensions;

internal sealed class EmptyResult : IResult
{
    public Task ExecuteAsync(BotRequestContext context)
    {
        return Task.CompletedTask;
    }
}
