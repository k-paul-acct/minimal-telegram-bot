namespace MinimalTelegramBot.Builder;

internal sealed class BotApplicationContainer
{
    public IBotApplicationBuilder? BotApplicationBuilder { get; set; }
    public Func<CancellationToken, Task>? DispatchFunc { get; set; }
}
