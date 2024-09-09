namespace MinimalTelegramBot.Results;

public interface IResult
{
    // TODO: Rename to Invoke.
    Task ExecuteAsync(BotRequestContext context);
}
