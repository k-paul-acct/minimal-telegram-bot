namespace MinimalTelegramBot.StateMachine.Abstractions;

// TODO: Docs.
/// <summary>
/// </summary>
public sealed class StateManagementOptions
{
    /// <summary>
    /// </summary>
    public StateTrackingStrategy TrackingStrategy { get; set; } =
        StateTrackingStrategy.DifferentiateUsers | StateTrackingStrategy.DifferentiateChats;

    /// <summary>
    /// </summary>
    public IStateRepository? Repository { get; set; }

    /// <summary>
    /// </summary>
    public IStateSerializer? Serializer { get; set; }

    /// <summary>
    /// </summary>
    public StateSerializerOptions? SerializerOptions { get; set; }

    /// <summary>
    /// </summary>
    public IStateTypeInfoResolver? TypeInfoResolver { get; set; }
}
