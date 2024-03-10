using TelegramBotFramework.Results.Extensions;

namespace TelegramBotFramework.Results;

public static class Results
{
    public static IResult Empty => new MessageResult(string.Empty);

    public static IResult Message(string message)
    {
        return new MessageResult(message);
    }
}