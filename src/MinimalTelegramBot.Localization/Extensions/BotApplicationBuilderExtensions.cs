using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Localization.Pipes;
using MinimalTelegramBot.Pipeline;

namespace MinimalTelegramBot.Localization.Extensions;

public static class BotApplicationBuilderExtensions
{
    public static IBotApplicationBuilder UseLocalization(this IBotApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.UsePipe<LocalizationPipe>();
        return app;
    }
}
