using System.Text.Encodings.Web;
using System.Text.Json;

namespace MinimalTelegramBot.StateMachine.Abstractions;

// TODO: Docs.
/// <summary>
/// </summary>
public sealed class StateSerializerOptions
{
    /// <summary>
    /// </summary>
    public StateSerializerOptions()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default)
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
    }

    /// <summary>
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; }
}
