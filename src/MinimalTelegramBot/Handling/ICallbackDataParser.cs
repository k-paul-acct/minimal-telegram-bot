namespace MinimalTelegramBot.Handling;

/// <summary>
///     Interface for parsing callback data into a statically-typed model.
/// </summary>
/// <typeparam name="TModel">The type of the model to parse the callback data into.</typeparam>
public interface ICallbackDataParser<out TModel>
{
    /// <summary>
    ///     Parses the callback data into a model of type <typeparamref name="TModel"/>.
    /// </summary>
    /// <param name="callbackData">The callback data to parse.</param>
    /// <returns>The parsed model of type <typeparamref name="TModel"/>.</returns>
    static abstract TModel Parse(string callbackData);
}
