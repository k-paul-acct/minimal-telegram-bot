using MinimalTelegramBot.Builder;
using MinimalTelegramBot.Localization.Pipes;
using MinimalTelegramBot.Pipeline;

namespace MinimalTelegramBot.Localization.Extensions;

public static class BotApplicationBuilderExtensions
{
    /// <summary>
    ///     Uses localization functionality in the app.
    /// </summary>
    /// <param name="app">The bot application builder.</param>
    /// <returns>The current instance of <see cref="IBotApplicationBuilder"/>.</returns>
    public static IBotApplicationBuilder UseLocalization(this IBotApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.UsePipe<LocalizationPipe>();
        return app;
    }
}
