using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Handling;

var builder = BotApplication.CreateBuilder(args);
var bot = builder.Build();

bot.HandleCommand("/start", () => "Hello, World!");

bot.Run();
