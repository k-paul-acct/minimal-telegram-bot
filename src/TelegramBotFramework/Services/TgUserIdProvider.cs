using TelegramBotFramework.Abstractions;

namespace TelegramBotFramework.Services;

/// <inheritdoc />
public class TelegramUserIdProvider : IUserIdProvider<long>
{
    private readonly BotRequestContext _context;

    public TelegramUserIdProvider(BotRequestContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public long GetUserId()
    {
        return _context.ChatId;
    }
}