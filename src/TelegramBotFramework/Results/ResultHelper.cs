namespace TelegramBotFramework.Results;

internal class ResultHelper
{
    public static Func<T, IResult> FromType<T>()
    {
        if (typeof(T) == typeof(string)) return x => Results.Message((x as string)!);
        return x =>
        {
            var message = x?.ToString();
            return message is null ? Results.Empty : Results.Message(message);
        };
    }
}