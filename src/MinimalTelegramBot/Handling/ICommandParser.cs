using MinimalTelegramBot.Localization.Abstractions;

namespace MinimalTelegramBot.Handling;

/// <summary>
///     Interface for parsing command message text into a statically-typed model.
/// </summary>
/// <typeparam name="TModel">The type of the model to parse the command message text into.</typeparam>
public interface ICommandParser<out TModel>
{
    /// <summary>
    ///     Parses the command message text into a model of type <typeparamref name="TModel"/>.
    /// </summary>
    /// <param name="command">The command message text to parse.</param>
    /// <param name="formatProvider">
    ///     The <see cref="IFormatProvider"/> of current user's <see cref="Locale"/> to use while parsing.
    ///     Passed if the <see cref="UseFormatProviderAttribute"/> is applied to the <typeparamref name="TModel"/> parameter
    ///     in the handler delegate.
    /// </param>
    /// <returns>The parsed model of type <typeparamref name="TModel"/>.</returns>
    static abstract TModel Parse(string command, IFormatProvider? formatProvider = null);
}
