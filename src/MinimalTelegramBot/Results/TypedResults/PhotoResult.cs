using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Results.TypedResults;

internal sealed class PhotoResult : FileResult
{
    public PhotoResult(Stream photoStream, string? caption = null) : base(photoStream, caption)
    {
    }

    public PhotoResult(string photoName, string? caption = null) : base(photoName, caption)
    {
    }

    public PhotoResult(Uri uri, string? caption) : base(uri, caption)
    {
    }

    protected override Task<Message> Send(BotRequestContext context, InputFile inputFile)
    {
        return context.Client.SendPhotoAsync(context.ChatId, inputFile, caption: Caption);
    }
}
