namespace TelegramBotFramework.Results;

public interface IResult
{
    Task ExecuteAsync(BotRequestContext context);
}