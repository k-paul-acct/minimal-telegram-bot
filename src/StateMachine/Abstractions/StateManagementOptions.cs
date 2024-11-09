namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
/// </summary>
public sealed class StateManagementOptions
{
    /// <summary>
    /// </summary>
    public StateTrackingStrategy StateTrackingStrategy { get; set; } =
        StateTrackingStrategy.DifferentiateUsers | StateTrackingStrategy.DifferentiateChats;

    /// <summary>
    /// </summary>
    public IUserStateRepository? Repository { get; set; }

    /// <summary>
    /// </summary>
    public IStateTypeInfoResolver? StateTypeInfoResolver { get; set; }

    /// <summary>
    /// </summary>
    public StateSerializationOptions? StateSerializationOptions { get; set; }
}
