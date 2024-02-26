using TelegramBotFramework.Pipeline;
using TelegramBotFramework.UsageExample.Services;

namespace TelegramBotFramework.UsageExample.Pipes;

public class CustomPipe : IPipe
{
    private readonly WeatherService _weatherService;

    public CustomPipe(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task InvokeAsync(BotRequestContext ctx, BotRequestDelegate next)
    {
        Console.WriteLine("    Pipe N2 Start");
        Console.WriteLine("    " + await _weatherService.GetWeather());
        await next(ctx);
        Console.WriteLine("    Pipe N2 End");
    }
}