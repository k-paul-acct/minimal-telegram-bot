using Telegram.Bot.Types;
using File = System.IO.File;

namespace MinimalTelegramBot.Results.Extensions;

internal abstract class FileResult : IResult
{
    private readonly string? _fileName;
    private readonly Stream? _fileStream;

    protected readonly string? Caption;

    protected FileResult(Stream fileStream, string? caption = null)
    {
        _fileStream = fileStream;
        Caption = caption;
    }

    protected FileResult(string fileName, string? caption = null)
    {
        _fileName = fileName;
        Caption = caption;
    }

    public Task ExecuteAsync(BotRequestContext context)
    {
        return _fileName is not null ? SendFromName(context) : SendFromStream(context, _fileStream!);
    }

    private Task SendFromName(BotRequestContext context)
    {
        var stream = File.OpenRead(_fileName!);
        return SendFromStream(context, stream);
    }

    private async Task SendFromStream(BotRequestContext context, Stream stream)
    {
        await using (stream)
        {
            var file = new InputFileStream(stream);
            await Send(context, file);
        }
    }

    protected abstract Task Send(BotRequestContext context, InputFile inputFile);
}