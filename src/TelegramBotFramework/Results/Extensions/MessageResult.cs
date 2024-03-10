using Telegram.Bot;

namespace TelegramBotFramework.Results.Extensions;

public class MessageResult : IResult
{
    private readonly string _message;

    public MessageResult(string message)
    {
        _message = message;
    }

    public async Task ExecuteAsync(BotRequestContext context)
    {
        await context.Client.SendTextMessageAsync(context.ChatId, _message);
    }
}