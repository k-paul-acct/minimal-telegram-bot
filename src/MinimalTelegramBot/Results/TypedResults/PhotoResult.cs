using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Results.TypedResults;

internal sealed class PhotoResult : FileResult
{
    public PhotoResult(Stream photoStream, string? caption = null) : base(photoStream, caption)
    {
    }

    public PhotoResult(string photoPath, string? caption = null) : base(photoPath, caption)
    {
    }

    public PhotoResult(Uri uri, string? caption) : base(uri, caption)
    {
    }

    protected override Task<Message> Send(BotRequestContext context, InputFile inputFile)
    {
        return context.Client.SendPhoto(context.ChatId, inputFile, Caption);
    }
}
