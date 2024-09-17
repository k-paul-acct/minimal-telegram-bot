using MinimalTelegramBot.Results.TypedResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace MinimalTelegramBot.Results;

/// <summary>
/// Represents a collection of static methods for generating different types of <see cref="IResult"/>.
/// </summary>
public static class Results
{
    /// <summary>
    /// Represents an empty <see cref="IResult"/> with no action.
    /// </summary>
    public static IResult Empty { get; } = new EmptyResult();

    /// <summary>
    /// Creates a new <see cref="IResult"/> that sends a message to the chat.
    /// </summary>
    /// <param name="message">Message text.</param>
    /// <param name="keyboard">Message keyboard.</param>
    /// <returns>Created <see cref="IResult"/>.</returns>
    public static IResult Message(string message, IReplyMarkup? keyboard = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        return new MessageResult(message, keyboard);
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> that sends a message to the chat as a reply.
    /// </summary>
    /// <param name="message">Message text.</param>
    /// <returns>Created <see cref="IResult"/>.</returns>
    public static IResult MessageReply(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        return new MessageResult(message, reply: true);
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> that edits the bot message contained in the update.
    /// </summary>
    /// <param name="message">New message text.</param>
    /// <param name="keyboard">New message keyboard.</param>
    /// <returns>Created <see cref="IResult"/>.</returns>
    public static IResult MessageEdit(string message, IReplyMarkup? keyboard = null)
    {
        ArgumentNullException.ThrowIfNull(message);

        return new MessageResult(message, keyboard, edit: true);
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> that answers the callback query.
    /// </summary>
    /// <returns>Created <see cref="IResult"/>.</returns>
    public static IResult CallbackAnswer()
    {
        return new CallbackAnswerResult();
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> that sends a photo to the chat.
    /// </summary>
    /// <param name="uri">URI of the photo relative to the wwwroot directory.</param>
    /// <param name="caption">Photo caption.</param>
    /// <returns>Created <see cref="IResult"/>.</returns>
    public static IResult Photo(Uri uri, string? caption = null)
    {
        ArgumentNullException.ThrowIfNull(uri);

        return new PhotoResult(uri, caption);
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> that sends a photo to the chat.
    /// </summary>
    /// <param name="photoName">Path of the photo file.</param>
    /// <param name="caption">Photo caption.</param>
    /// <returns>Created <see cref="IResult"/>.</returns>
    // TODO: Rename photoName to photoPath.
    public static IResult Photo(string photoName, string? caption = null)
    {
        ArgumentNullException.ThrowIfNull(photoName);

        return new PhotoResult(photoName, caption);
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> that sends a photo to the chat.
    /// </summary>
    /// <param name="photoStream">Stream representing the photo.</param>
    /// <param name="caption">Photo caption.</param>
    /// <returns>Created <see cref="IResult"/>.</returns>
    public static IResult Photo(Stream photoStream, string? caption = null)
    {
        ArgumentNullException.ThrowIfNull(photoStream);

        return new PhotoResult(photoStream, caption);
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> that sends a document to the chat.
    /// </summary>
    /// <param name="documentName">Path of the document file.</param>
    /// <param name="caption">Document caption.</param>
    /// <returns>Created <see cref="IResult"/>.</returns>
    // TODO: Rename documentName to documentPath.
    public static IResult Document(string documentName, string? caption = null)
    {
        ArgumentNullException.ThrowIfNull(documentName);

        return new DocumentResult(documentName, caption);
    }

    /// <summary>
    /// Creates a new <see cref="IResult"/> that sends a document to the chat.
    /// </summary>
    /// <param name="documentStream">Stream representing the document.</param>
    /// <param name="caption">Document caption.</param>
    /// <returns>Created <see cref="IResult"/>.</returns>
    public static IResult Document(Stream documentStream, string? caption = null)
    {
        ArgumentNullException.ThrowIfNull(documentStream);

        return new DocumentResult(documentStream, caption);
    }
}
