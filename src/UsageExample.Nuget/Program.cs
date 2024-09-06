using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UsageExample.Nuget.Services;

var builder = BotApplication.CreateBuilder(args);

builder.SetBotToken(builder.Configuration["BotToken"]!);

builder.ConfigureReceiverOptions(options =>
{
    options.AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery,];
    options.DropPendingUpdates = true;
});

builder.Services.AddStateMachine();
builder.Services.AddSingleLocale(new Locale("ru"), locale => locale.EnrichFromFile("Localization/ru.yaml"));
builder.Services.AddScoped<WeatherService>();

var app = builder.Build();

app.UseCallbackAutoAnswering();

app.Handle((ILocalizer localizer) =>
{
    var helloText = localizer["HelloText"];
    var helloButton = localizer["Button.Hello"];
    var catButton = localizer["Button.Cat"];
    var keyboard = new ReplyKeyboardMarkup(new KeyboardButton(helloButton), new KeyboardButton(catButton)) { ResizeKeyboard = true, };

    return Results.Message(helloText, keyboard);
}).FilterCommand("/start");

app.Handle(() =>
{
    var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Send photo!", "Photo"));
    return Results.MessageEdit("Button pressed", keyboard);
}).FilterCallbackData(x => x == "Hello");

app.Handle(() => Results.Photo("cat.jpeg", "Little cat"))
    .FilterCallbackData(x => x == "Photo");

app.Handle(() => Results.MessageReply("I'm replied!"))
    .FilterText(x => x.Equals("reply", StringComparison.OrdinalIgnoreCase));

app.Handle(async (string messageText, WeatherService weatherService) =>
{
    var weather = await weatherService.GetWeather();
    var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Hello", "Hello"));
    return ($"Hello, {messageText}, weather is {weather}", keyboard);
}).FilterUpdateType(x => x == UpdateType.Message);

app.Run();
