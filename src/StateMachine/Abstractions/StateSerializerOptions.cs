using System.Text.Encodings.Web;
using System.Text.Json;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Provides options for state serialization.
/// </summary>
public sealed class StateSerializerOptions
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="StateSerializerOptions"/>.
    /// </summary>
    public StateSerializerOptions()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
    }

    /// <summary>
    ///     Gets or sets the JSON serializer options for state properties.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; }
}
