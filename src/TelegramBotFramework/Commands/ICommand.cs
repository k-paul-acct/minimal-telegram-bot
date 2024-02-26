using TelegramBotFramework.Pipeline;

namespace TelegramBotFramework.Commands;

public interface ICommand
{
    Task ExecuteAsync(BotRequestContext ctx, CancellationToken cancellationToken = default);
}