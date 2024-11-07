using System.Text.Json;

namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
public sealed class StateManagementOptions
{
    /// <summary>
    /// </summary>
    public IUserStateRepository? Repository { get; set; }

    /// <summary>
    /// </summary>
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
}
