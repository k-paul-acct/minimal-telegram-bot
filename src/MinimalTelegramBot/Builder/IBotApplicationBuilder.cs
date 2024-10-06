namespace MinimalTelegramBot.Builder;

/// <summary>
///     Provides the mechanisms to configure a bot application's request pipeline.
/// </summary>
public interface IBotApplicationBuilder
{
    /// <summary>
    ///     Gets the <see cref="IServiceProvider"/> that provides access to the underlying bot application's service container.
    /// </summary>
    IServiceProvider Services { get; }

    /// <summary>
    ///     Gets a key/value collection that can be used to add additional metadata while building.
    /// </summary>
    IDictionary<string, object?> Properties { get; }

    /// <summary>
    ///     Adds a middleware to the bot application's request pipeline.
    /// </summary>
    /// <param name="pipe">A function that takes a <see cref="BotRequestDelegate"/> and returns a <see cref="BotRequestDelegate"/>.</param>
    /// <returns>The <see cref="IBotApplicationBuilder"/>.</returns>
    IBotApplicationBuilder Use(Func<BotRequestDelegate, BotRequestDelegate> pipe);

    /// <summary>
    ///     Builds the delegate used by bot application to process bot requests.
    /// </summary>
    /// <returns>The bot request handling delegate.</returns>
    BotRequestDelegate Build();
}
