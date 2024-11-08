<img alt="" src="./logo/package-icon.png" width="96"/>

# Welcome to the Minimal Telegram Bot project

[![latest version](https://img.shields.io/nuget/v/MinimalTelegramBot)](https://www.nuget.org/packages/MinimalTelegramBot)
[![Telegram Bot API](https://img.shields.io/badge/Telegram_Bot_API-7.11-blue)](https://core.telegram.org/bots/api-changelog#october-31-2024)

Minimal Telegram Bot is a modern .NET framework for building Telegram Bots using simple and concise syntax inspired by
ASP.NET Core Minimal APIs.

Join the project's [Telegram channel](https://t.me/minimal_telegram_bot_channel) to stay tuned or ask for help in the
project's [Telegram group](https://t.me/minimal_telegram_bot_group).

## Intentions
The development of a Minimal Telegram Bot project was started as an attempt to solve the problem with the lack of a
single convenient solution for Telegram bot development on the .NET platform.

A great community-approved Telegram Bot API wrapper already exists in .NET. However, creating bots for more complex
cases using only a bare-bones wrapper soon becomes cumbersome and unsupportable. Existing solutions that solve such a
problem were made very focused on the specific author's problem and not fit well to wider scope of demands,
and some of the solutions are abandoned as of today.

With some experience in development and other languages, I developed a proof of concept where writing
bots was made way easier, hiding unnecessary boilerplate details. The developer and consumer of this project now only
need to focus on their tasks, write the code that is needed here and now to solve the business tasks and not worry
about the rest.

## Considerations
Detailed documentation is still in progress with framework's public APIs so some changes are possible in the future.
The project roadmap includes considerations for changing the policy of support and updates as well as the communication
practices with the audience if it gains enough audience.

At the moment, this framework already has enough features and customization mechanisms to use in real projects.
Future plans are to create more public API methods for user convenience, tailor the API and provide full documentation.

## Features
- Support for the latest Telegram Bot API version, receiving fast updates thanks to
  [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot), the most popular .NET Client for
  [Telegram Bot API](https://core.telegram.org/bots/api)

### Framework features
- Fully parallel Telegram updates handling
- Pipeline ([middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware)) building just like in
  ASP.NET Core
- Powerful and customizable updates handling and filtering similar to
  [ASP.NET Core Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- Integration of [dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection),
  configuration and logging from ASP.NET Core

### Extensions
- Support for the Finite State Machine concept for Telegram bots using StateMachine extension (WIP)
- Built-in bot localization features using Localization extension (WIP)

### Getting updates from Telegram
- Support [getting updates](https://core.telegram.org/bots/faq#how-do-i-get-updates) via both webhook and polling
- Provides direct [responses](https://core.telegram.org/bots/faq#how-can-i-make-requests-in-response-to-updates) to
  webhook requests

## Getting started
### Prerequisites
This project is using .NET 8 for now, so be sure to [install](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
the appropriate version of .NET, and that's all you need to start developing.

### New project
Start by creating an "Empty" ASP.NET Core application using the command line:
```sh
dotnet new web -n MyBot -f net8.0
```
Or create a new ASP.NET Core web application in your IDE, and choose "Empty" for the project template.

### Installation
Minimal Telegram Bot library is available on [NuGet](https://www.nuget.org/packages/MinimalTelegramBot) so you can
install the library using NuGet package manager in your IDE:
```sh
dotnet add package MinimalTelegramBot
```
In addition, you can add some extension packages that provide Localization and State Machine features:
```sh
dotnet add package MinimalTelegramBot.Localization
dotnet add package MinimalTelegramBot.StateMachine
```

## Usage
### Basic setup
In your `Program.cs` you can use following basic setup:
```csharp
using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Handling;

var builder = BotApplication.CreateBuilder(args);
var bot = builder.Build();

bot.HandleCommand("/start", () => "Hello, World!");

bot.Run();
```
In the code above we are creating `BotApplication` using builder just like in web applications. When the `bot` instance
are set up and ready for handling configuration, we call `HandleCommand()` with specified `/start` command and return
`Hello, World!` message as a result.

By default, [polling](https://core.telegram.org/bots/faq#how-do-i-get-updates) method for getting updates from Telegram
is used because it's faster to start development with polling method and polling requires less effort to configure.
Bot token is specified in configuration with `"BotToken"` key. For local development you can set your bot token using
[dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) tool:
```sh
dotnet user-secrets init
dotnet user-secrets set "BotToken" "your bot token"
```
In other environments you can use `.env` files or just set bot token in `appsettings.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "BotToken": "your bot token"
}
```
> Ensure that your bot token is not included in version control to prevent unauthorized access to your bot.

If you have any trouble with telegram bot token or [BotFather](https://t.me/botfather), read the
[official tutorial](https://core.telegram.org/bots/tutorial#obtain-your-bot-token).

When all configuration is complete, just run program with `dotnet run` or via IDE's interface and test your bot.

## Advanced handling examples
### Localizer usage
Localization is essential when creating bots for users across multiple regions.
Here, the localizer service allows you to retrieve messages dynamically based on the user's locale.
How to add localizer in bot app with single locale before building the bot app:
```csharp
builder.Services.AddSingleLocale(
    new Locale("en"),
    locale => locale.EnrichFromFile("Localization/en.yaml"));
```
How to use localizer services after building bot app:
```csharp
bot.UseLocalization();
```
To use different languages, you need to implement `IUserLocaleProvider` interface,
in that way you provide the context for resolving user's locale.
You can find more details about adding localization and different languages in the
[code samples](./src/UsageExample/Program.cs).

The command below uses localizer to respond to the `/start` command with a `"HelloText"` message retrieved and
a dynamically generated keyboard based on the user's locale from method `MenuKeyboard()`.
[See more](https://telegrambots.github.io/book/2/reply-markup.html) on keyboards.
```csharp
bot.HandleCommand("/start", (ILocalizer localizer) =>
{
    var helloText = localizer["HelloText"];
    var keyboard = MenuKeyboard(localizer);
    return Results.Message(helloText, keyboard);
});
```

### Callback data handling
This handler handles callback data `"Photo"` and responses with photo and description.

Here, general `Handle()` method is used with `FilterCallbackData()` filter.

Note, `wwwroot` folder comes from ASP.NET Core static file serving.
```csharp
bot.Handle(() =>
        Results.Photo("wwwroot/cat.jpeg", "Little cat"))
    .FilterCallbackData(x => x == "Photo");
```

### Parameter parsing
This example handles the `/add` command by parsing user input into `AddCommandModel` and responding with a sum
of two parsed numbers or error message, depending on the input's validity.
Example: `/add 1.3 5.7` outputs `7`.
Attribute `[UseFormatProvider]` allows culture-specific number parsing like comma- or point-based decimal separator.
```csharp
bot.HandleCommand("/add", (
    [UseFormatProvider] AddCommandModel model,
    ILocalizer localizer) =>
{
    return model.IsError
        ? localizer["ParsingError"]
        : localizer["AddCommandTemplate", model.Result];
});
```
Here example of `AddCommandModel` class, note that implementing an interface `ICommandParser`.
This interface is essential for ensuring that command will be parsed.
```csharp
public class AddCommandModel : ICommandParser<AddCommandModel>
{
    public double A { get; set; }
    public double B { get; set; }
    public bool IsError { get; set; }
    public double Result => A + B;

    public static AddCommandModel Parse(
        string command,
        IFormatProvider? formatProvider = null)
    {
        var parts = command.Split(' ');

        if (parts.Length < 3 ||
            !double.TryParse(parts[1], formatProvider, out var a) ||
            !double.TryParse(parts[2], formatProvider, out var b))
        {
            return new AddCommandModel { IsError = true, };
        }

        return new AddCommandModel { A = a, B = b, };
    }
}
```
Callback data parsing is also supported using a similar API.

### Dependency injection and keyed services
In this example, we handle commands that retrieve data from a specific service registered in DI container with a key.
The `NameService` is used here to provide different names depending on the command.
By using the `FromKeyedServices` attribute, we dynamically inject the appropriate service into the handler based on the
command.

`FromKeyedServices`
[attribute](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection#keyed-services) allows you
to dynamically inject services registered with specific keys, such as `FirstName`, or `LastName` in this example.

Standard dependency injection techniques are also supported, handling context is a scope.
```csharp
bot.HandleCommand("/firstname",
    ([FromKeyedServices("FirstName")] NameService nameService) =>
        nameService.Name);

bot.HandleCommand("/lastname",
    ([FromKeyedServices("LastName")] NameService nameService) =>
        nameService.Name);
```

### Async weather service example
This example demonstrates how to handle complex interactions asynchronously,
using a weather service to fetch real-time data.

Here, general `Handle()` method is used with `FilterCallbackData()` filter.

Note, here `string messageText` directly injected from incoming message, and
handler catches all messages by using `FilterUpdateType(UpdateType.Message)` filter.
```csharp
bot.Handle(async (string messageText, WeatherService weatherService) =>
{
    var weather = await weatherService.GetWeather();
    var keyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Hello", "Hello"));
    return ($"Hello, {messageText}, weather is {weather}", keyboard);
}).FilterUpdateType(UpdateType.Message);
```

### Error handling
This example shows default error handling. Command `/throw` randomly decides to throw an exception
and default error handling mechanism gracefully caches an exception and logs it preventing bot app from unexpected
shutdown. If no error occurs, the bot responds with message 'Ok'.
```csharp
bot.HandleCommand("/throw", () =>
{
    if (Random.Shared.Next(0, 2) == 0)
    {
        throw new Exception("Error");
    }

    return Results.Message("Ok");
});
```
If you need custom error handling, you can wrap bot app execution pipeline (middleware) in your own middleware
like this:
```csharp
bot.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception ex)
    {
        // Custom exception handling.
    }
});
```
Ensure that this middleware is placed before other handlers in the pipeline.

## More advanced use-cases
If you need examples of more complex use-cases and features, you can check code example
[here](https://github.com/k-paul-acct/minimal-telegram-bot/blob/main/src/UsageExample.Nuget/Program.cs) or ask any
question in [Telegram group](https://t.me/minimal_telegram_bot_group).

## Additional resources
Check out some resources that can help you develop good bots:
- [Code samples](./src)
- Localization extension [NuGet package](https://www.nuget.org/packages/MinimalTelegramBot.Localization)
- StateMachine extension [NuGet package](https://www.nuget.org/packages/MinimalTelegramBot.StateMachine)
- Telegram.Bot API wrapper [GitHub](https://github.com/TelegramBots/Telegram.Bot)
- Telegram.Bot API wrapper [documentation](https://telegrambots.github.io/book/)
- Telegram.Bot [group](https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA)
- Project's [Telegram channel](https://t.me/minimal_telegram_bot_channel)
- Project's [Telegram group](https://t.me/minimal_telegram_bot_group)

## Updates

Currently, the best way to be up to date is using the latest NuGet package versions.
There are plans for major feature updates or breaking changes announcements in
[Telegram channel](https://t.me/minimal_telegram_bot_channel) but keep in mind that the project is still under active
development and is in its pre-1.0.0 version stage.

## Contributing and feedback

I welcome community pull requests for bug fixes, enhancements, and documentation.
See [How to contribute](./.github/CONTRIBUTING.md) for more information.

If you have a specific question about using this project, I encourage you to ask it on
[Telegram group](https://t.me/minimal_telegram_bot_group).
If you encounter a bug or would like to request a feature, submit a GitHub issue.
