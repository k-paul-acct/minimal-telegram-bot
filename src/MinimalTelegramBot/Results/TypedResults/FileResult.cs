using Telegram.Bot.Types;
using File = System.IO.File;

namespace MinimalTelegramBot.Results.TypedResults;

internal abstract class FileResult : IResult
{
    private readonly string? _filePath;
    private readonly Uri? _uri;
    private readonly Stream? _fileStream;

    protected readonly string? Caption;

    protected FileResult(Stream fileStream, string? caption = null)
    {
        _fileStream = fileStream;
        Caption = caption;
    }

    protected FileResult(string filePath, string? caption = null)
    {
        _filePath = filePath;
        Caption = caption;
    }

    protected FileResult(Uri uri, string? caption = null)
    {
        _uri = uri;
        Caption = caption;
    }

    public Task ExecuteAsync(BotRequestContext context)
    {
        if (_uri is not null)
        {
            return SendFromUri(context);
        }

        return _fileStream is null ? SendFromName(context) : SendFromStream(context, _fileStream);
    }

    private Task<Message> SendFromName(BotRequestContext context)
    {
        var stream = File.OpenRead(_filePath!);
        return SendFromStream(context, stream);
    }

    private Task<Message> SendFromUri(BotRequestContext context)
    {
        var baseUri = (Uri)context._properties["__WebhookUrl"]!;
        var fullUri = new Uri(baseUri, _uri!);
        var file = new InputFileUrl(fullUri);
        return Send(context, file);
    }

    private Task<Message> SendFromStream(BotRequestContext context, Stream stream)
    {
        context.RegisterForDispose(stream);
        var file = new InputFileStream(stream);
        return Send(context, file);
    }

    protected abstract Task<Message> Send(BotRequestContext context, InputFile inputFile);
}
