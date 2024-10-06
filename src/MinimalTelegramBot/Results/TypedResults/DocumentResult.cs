using Telegram.Bot;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Results.TypedResults;

internal sealed class DocumentResult : FileResult
{
    public DocumentResult(Stream documentStream, string? caption = null) : base(documentStream, caption)
    {
    }

    public DocumentResult(string documentPath, string? caption = null) : base(documentPath, caption)
    {
    }

    protected override Task<Message> Send(BotRequestContext context, InputFile inputFile)
    {
        return context.Client.SendDocumentAsync(context.ChatId, inputFile, caption: Caption);
    }
}
