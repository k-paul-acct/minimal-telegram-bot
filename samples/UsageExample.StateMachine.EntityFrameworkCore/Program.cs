using Microsoft.EntityFrameworkCore;
using MinimalTelegramBot;
using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Handling;
using MinimalTelegramBot.StateMachine.Extensions;
using MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;
using Telegram.Bot.Types.Enums;
using UsageExample.StateMachine.EntityFrameworkCore;

var builder = BotApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=db.db"));

builder.Services
    .AddStateMachine()
    .PersistStatesToDbContext<AppDbContext>()
    .WithHybridCache();

var bot = builder.Build();

using (var scope = bot.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

bot.UseStateMachine();

bot.HandleCommand("/start", () => "Send command /name to set your full name");

bot.HandleCommand("/name", async (BotRequestContext context) =>
{
    var state = new FullNameFsm.EnteringFirstNameState();
    await context.SetState(state);
    return "Enter your first name";
});

bot.HandleUpdateType(UpdateType.Message, async (string messageText, BotRequestContext context) =>
{
    var enteringLastNameState = new FullNameFsm.EnteringLastNameState
    {
        FirstName = messageText,
    };

    await context.SetState(enteringLastNameState);

    return "Enter your last name";
}).FilterState<FullNameFsm.EnteringFirstNameState>();

bot.HandleUpdateType(UpdateType.Message, async (string messageText, BotRequestContext context,
    FullNameFsm.EnteringLastNameState state) =>
{
    await context.DropState();
    return $"Your full name: {state.FirstName} {messageText}";
}).FilterState<FullNameFsm.EnteringLastNameState>();

bot.Run();
