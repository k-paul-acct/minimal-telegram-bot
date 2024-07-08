namespace MinimalTelegramBot.Results;

public interface IResult
{
    Task ExecuteAsync(BotRequestContext context);
}