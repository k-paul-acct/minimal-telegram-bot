using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace MinimalTelegramBot.Results.Extensions;

public class PhotoResult : IResult
{
    private readonly string? _caption;
    private readonly string? _photoName;
    private readonly Stream? _photoStream;

    public PhotoResult(Stream photoStream, string? caption = null)
    {
        _photoStream = photoStream;
        _caption = caption;
    }

    public PhotoResult(string photoName, string? caption = null)
    {
        _photoName = photoName;
        _caption = caption;
    }

    public Task ExecuteAsync(BotRequestContext context)
    {
        return _photoName is not null ? SendFromName(context) : SendFromStreamAndDispose(context, _photoStream!);
    }

    private async Task SendFromName(BotRequestContext context)
    {
        var stream = File.OpenRead(_photoName!);
        await SendFromStreamAndDispose(context, stream);
    }

    private async Task SendFromStreamAndDispose(BotRequestContext context, Stream stream)
    {
        await using (stream)
        {
            var photo = new InputFileStream(stream);
            await context.Client.SendPhotoAsync(context.ChatId, photo, caption: _caption);
        }
    }
}