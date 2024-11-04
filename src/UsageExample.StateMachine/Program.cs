using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Handling;
using MinimalTelegramBot.StateMachine.Abstractions;
using MinimalTelegramBot.StateMachine.Extensions;
using Telegram.Bot.Types.Enums;
using UsageExample.StateMachine.States;

var builder = BotApplication.CreateBuilder(args);

builder.Services.AddStateMachine();

var bot = builder.Build();

bot.UseStateMachine();

bot.HandleCommand("/start", () => "Send command /name to set your full name");

bot.HandleCommand("/name", (IStateMachine stateMachine) =>
{
    stateMachine.SetState(FullNameStateMachine.EnteringFirstName);
    return "Enter your first name";
});

bot.HandleUpdateType(UpdateType.Message, (string messageText, IStateMachine stateMachine) =>
{
    var enteringLastNameState = FullNameStateMachine.EnteringLastName;

    enteringLastNameState.Data["FirstName"] = messageText;

    stateMachine.SetState(enteringLastNameState);

    return "Enter your last name";
}).FilterState(FullNameStateMachine.EnteringFirstName);

bot.HandleUpdateType(UpdateType.Message, (string messageText, IStateMachine stateMachine) =>
{
    var state = stateMachine.GetState();
    if (state is null)
    {
        return "Error";
    }

    var firstName = state.Data["FirstName"] as string;

    stateMachine.DropState();

    return $"Your full name: {firstName} {messageText}";
}).FilterState(FullNameStateMachine.EnteringLastName);

bot.Run();
