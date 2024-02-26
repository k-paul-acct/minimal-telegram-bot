namespace TelegramBotFramework.Commands;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute
{
    public readonly string Command;

    public CommandAttribute(string command)
    {
        Command = command;
    }
}