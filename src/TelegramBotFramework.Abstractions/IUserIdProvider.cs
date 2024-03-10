namespace TelegramBotFramework.Abstractions;

public interface IUserIdProvider<out TUserId>
{
    TUserId GetUserId();
}