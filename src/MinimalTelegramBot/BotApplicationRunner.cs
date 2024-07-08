namespace MinimalTelegramBot;

internal static class BotApplicationRunner
{
    public static void Run(BotApplication app)
    {
        ((IBotApplicationBuilder)app).Build();
        app.RunBot();
        app.Host.Run();
    }
}