using MinimalTelegramBot.Results.Extensions;
using Telegram.Bot.Types.ReplyMarkups;

namespace MinimalTelegramBot.Results;

public static class Results
{
    public static IResult Empty { get; } = new EmptyResult();

    public static IResult Message(string message, IReplyMarkup? keyboard = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        return new MessageResult(message, keyboard);
    }

    public static IResult MessageReply(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        return new MessageResult(message, reply: true);
    }

    public static IResult MessageEdit(string message, IReplyMarkup? keyboard = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        return new MessageResult(message, keyboard, edit: true);
    }

    public static IResult CallbackAnswer()
    {
        return new CallbackAnswerResult();
    }

    public static IResult Photo(string photoName, string? caption = null)
    {
        ArgumentNullException.ThrowIfNull(photoName);

        return new PhotoResult(photoName, caption);
    }

    public static IResult Photo(Stream photoStream, string? caption = null)
    {
        ArgumentNullException.ThrowIfNull(photoStream);

        return new PhotoResult(photoStream, caption);
    }

    public static IResult Document(string documentName, string? caption = null)
    {
        ArgumentNullException.ThrowIfNull(documentName);

        return new DocumentResult(documentName, caption);
    }

    public static IResult Document(Stream documentStream, string? caption = null)
    {
        ArgumentNullException.ThrowIfNull(documentStream);

        return new DocumentResult(documentStream, caption);
    }
}
