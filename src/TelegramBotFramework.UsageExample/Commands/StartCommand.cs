using Telegram.Bot;
using TelegramBotFramework.Commands;
using TelegramBotFramework.Pipeline;
using TelegramBotFramework.UsageExample.Services;

namespace TelegramBotFramework.UsageExample.Commands;

[Command("/start")]
public class StartCommand : ICommand
{
    private readonly WeatherService _weatherService;

    public StartCommand(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task ExecuteAsync(BotRequestContext ctx, CancellationToken cancellationToken = default)
    {
        ctx.UpdateHandlingStarted = true;

        var weather = await _weatherService.GetWeather();
        var chatId = ctx.Update.Message?.Chat.Id ?? 0;
        await ctx.Client.SendTextMessageAsync(chatId, weather, cancellationToken: cancellationToken);
    }
}