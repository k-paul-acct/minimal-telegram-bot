namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Options for managing states in the <see cref="IStateMachine"/>.
/// </summary>
public sealed class StateManagementOptions
{
    /// <summary>
    ///     Gets or sets the strategy for tracking states.
    /// </summary>
    public StateTrackingStrategy TrackingStrategy { get; set; } =
        StateTrackingStrategy.DifferentiateUsers | StateTrackingStrategy.DifferentiateChats;

    /// <summary>
    ///     Gets or sets the repository for storing states.
    /// </summary>
    public IStateRepository? Repository { get; set; }

    /// <summary>
    ///     Gets or sets the serializer for state objects.
    /// </summary>
    public IStateSerializer? Serializer { get; set; }

    /// <summary>
    ///     Gets or sets the options for the state serializer.
    /// </summary>
    public StateSerializerOptions? SerializerOptions { get; set; }

    /// <summary>
    ///     Gets or sets the resolver for state type information.
    /// </summary>
    public IStateTypeInfoResolver? TypeInfoResolver { get; set; }
}
