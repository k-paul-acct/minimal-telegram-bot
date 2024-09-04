using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Results.Extensions;

internal sealed class PhotoResult : FileResult
{
    public PhotoResult(Stream photoStream, string? caption = null) : base(photoStream, caption)
    {
    }

    public PhotoResult(string photoName, string? caption = null) : base(photoName, caption)
    {
    }

    protected override Task Send(BotRequestContext context, InputFile inputFile)
    {
        return context.Client.SendPhotoAsync(context.ChatId, inputFile, caption: Caption);
    }
}