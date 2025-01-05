namespace MinimalTelegramBot.StateMachine.Abstractions;

/// <summary>
///     Specifies the strategy to differentiate states for users in chats.
/// </summary>
[Flags]
public enum StateTrackingStrategy
{
    /// <summary>
    ///     Differentiates only users, tries to base on <c>user_id</c>.
    /// </summary>
    DifferentiateUsers = 1,

    /// <summary>
    ///     Differentiates only chats, tries to base on <c>chat_id</c>.
    /// </summary>
    DifferentiateChats = 2,

    /// <summary>
    ///     Differentiates only topics, relevant for chats with topics.
    /// </summary>
    /// <remarks>
    ///     Implicitly implies <see cref="StateTrackingStrategy.DifferentiateChats"/>
    /// </remarks>
    DifferentiateTopics = 4 | 2,
}
