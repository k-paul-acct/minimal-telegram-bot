using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MinimalTelegramBot.Results.Extensions;

public sealed class MessageResult : IResult
{
    private readonly bool _edit;
    private readonly string _message;
    private readonly bool _reply;
    private readonly IReplyMarkup? _replyMarkup;

    public MessageResult(string message, IReplyMarkup? keyboard = null, bool reply = false, bool edit = false)
    {
        _message = message;
        _replyMarkup = keyboard;
        _reply = reply;
        _edit = edit;
    }

    public Task ExecuteAsync(BotRequestContext context)
    {
        if (_edit)
        {
            return Edit(context);
        }

        return _reply ? Reply(context) : Send(context);
    }

    private async Task Edit(BotRequestContext context)
    {
        if (_replyMarkup is InlineKeyboardMarkup inline)
        {
            var messageId = context.Update.CallbackQuery!.Message!.MessageId;
            await context.Client.EditMessageTextAsync(context.ChatId, messageId, _message, replyMarkup: inline);
        }
    }

    private async Task Reply(BotRequestContext context)
    {
        var messageId = context.Update.Message!.MessageId;
        var replyParameters = new ReplyParameters { ChatId = context.ChatId, MessageId = messageId, };

        await context.Client.SendTextMessageAsync(context.ChatId, _message, replyParameters: replyParameters,
            replyMarkup: _replyMarkup);
    }

    private async Task Send(BotRequestContext context)
    {
        await context.Client.SendTextMessageAsync(context.ChatId, _message, replyMarkup: _replyMarkup);
    }
}