using System.Text.Json;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
public sealed class StateSerializationOptions
{
    /// <summary>
    /// </summary>
    public static StateSerializationOptions Default => new();

    /// <summary>
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = JsonSerializerOptions.Default;
}
