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
    public IStateSerializerContext? StateSerializerContext { get; set; }
}
