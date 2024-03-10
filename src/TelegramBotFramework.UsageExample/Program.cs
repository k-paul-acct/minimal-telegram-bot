using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework;
using TelegramBotFramework.Settings;
using TelegramBotFramework.UsageExample.Services;

var builder = BotApplication.CreateBuilder(new BotApplicationBuilderOptions
{
    Args = args,
    ReceiverOptions = new ReceiverOptions
    {
        AllowedUpdates = [UpdateType.Message,],
        ThrowPendingUpdates = true,
    },
});

builder.HostBuilder.Services.AddScoped<WeatherService>();

builder.SetTokenFromConfiguration("BotToken");

var app = builder.Build();

app.Handle(async (string messageText, WeatherService weatherService) =>
{
    var weather = await weatherService.GetWeather();
    return $"Hello, {messageText}, weather is {weather}";
});

app.Run();