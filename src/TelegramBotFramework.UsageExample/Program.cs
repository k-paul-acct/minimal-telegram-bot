using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBotFramework;
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

builder.SetTokenFromConfiguration("BotToken").AddCommands();

var app = builder.Build();

app.Run();