using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Handling;
using MinimalTelegramBot.Handling.Filters;
using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.Localization.Extensions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UsageExample.CallbackModels;
using UsageExample.CommandModels;
using UsageExample.Services;
using Results = MinimalTelegramBot.Results.Results;

// TODO: More Result templates.

var builder = BotApplication.CreateBuilder(args);

builder.Services.AddSingleLocale(new Locale("ru"), locale => locale.EnrichFromFile("Localization/ru.yaml"));

builder.Services.AddScoped<WeatherService>();
builder.Services.AddKeyedSingleton("FirstName", new NameService { Name = "First Name", });
builder.Services.AddKeyedSingleton("LastName", new NameService { Name = "Last Name", });

var app = builder.Build();

app.UsePolling();
//app.UseWebhook(new WebhookOptions { Url = "", });

app.UseCallbackAutoAnswering();

app.UseLocalization();

app.Handle((ILocalizer localizer) =>
{
    var helloText = localizer["HelloText"];
    var keyboard = MenuKeyboard(localizer);

    return Results.Message(helloText, keyboard);
}).FilterCommand("/start");

app.Handle(([UseFormatProvider] AddCommandModel model, ILocalizer localizer) => model.IsError
    ? localizer["ParsingError"]
    : localizer["AddCommandTemplate", model.Result]).FilterCommand("/add");

app.Handle(([FromKeyedServices("FirstName")] NameService nameService) => nameService.Name).FilterCommand("/firstname");

app.Handle(([FromKeyedServices("LastName")] NameService nameService) => nameService.Name).FilterCommand("/lastname");

app.HandleCommand("/throw", () =>
{
    if (Random.Shared.Next(0, 2) == 0)
    {
        throw new Exception("Error");
    }

    return Results.Message("Ok");
});

app.Handle((ILocalizer localizer) =>
{
    var menuText = localizer["Menu"];
    var keyboard = MenuKeyboard(localizer);

    return Results.Message(menuText, keyboard);
}).FilterCallbackData(x => x == "Menu");

app.Handle((ILocalizer localizer) =>
{
    var backText = localizer["Button.Back"];

    var model = new PaginationModel { PageNumber = 1, };
    var keyboard = model.GetPageKeyboard(backText, "Menu");

    return Results.Message("Page 1", keyboard);
}).FilterMessageTextWithLocalizer("Button.Page");

app.Handle((PaginationModel model, ILocalizer localizer) =>
{
    var backText = localizer["Button.Back"];
    var pageText = $"Page {model.PageNumber}";
    var keyboard = model.GetPageKeyboard(backText, "Menu");

    return keyboard is null
        ? Results.CallbackAnswer()
        : Results.MessageEdit(pageText, keyboard);
}).FilterCallbackData(x => x.StartsWith(PaginationModel.CallbackPrefix));

app.Handle(() =>
{
    var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Send photo!", "Photo"));
    return Results.MessageEdit("Button pressed", keyboard);
}).FilterCallbackData(x => x == "Hello");

app.Handle(() => Results.Photo(new Uri("cat.jpeg", UriKind.Relative), "Little cat")).FilterCallbackData(x => x == "Photo");

app.Handle(() => Results.MessageReply("I'm replied!")).FilterMessageText(x => x.Equals("reply", StringComparison.OrdinalIgnoreCase));

app.Handle(async (string messageText, WeatherService weatherService) =>
{
    var weather = await weatherService.GetWeather();
    var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Hello", "Hello"));
    return ($"Hello, {messageText}, weather is {weather}", keyboard);
}).FilterUpdateType(x => x == UpdateType.Message);

app.Run();

return;

static ReplyKeyboardMarkup MenuKeyboard(ILocalizer localizer)
{
    var helloButton = localizer["Button.Hello"];
    var catButton = localizer["Button.Cat"];
    var pagesButton = localizer["Button.Page"];

    return new ReplyKeyboardMarkup(new[]
    {
        new KeyboardButton(helloButton),
        new KeyboardButton(catButton),
        new KeyboardButton(pagesButton),
    }) { ResizeKeyboard = true, };
}
