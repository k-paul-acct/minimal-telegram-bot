namespace MinimalTelegramBot.StateMachine.Abstractions;

// TODO: Docs.
/// <summary>
/// </summary>
[Flags]
public enum StateTrackingStrategy
{
    /// <summary>
    /// </summary>
    DifferentiateUsers = 1,

    /// <summary>
    /// </summary>
    DifferentiateChats = 2,

    /// <summary>
    /// </summary>
    DifferentiateTopics = 4 | 2,
}
