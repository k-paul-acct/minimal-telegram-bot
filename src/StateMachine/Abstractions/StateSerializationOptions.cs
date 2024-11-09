using System.Text.Json;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
public sealed class StateSerializationOptions
{
    /// <summary>
    /// </summary>
    public StateSerializationOptions()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default);
    }

    /// <summary>
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; }
}
