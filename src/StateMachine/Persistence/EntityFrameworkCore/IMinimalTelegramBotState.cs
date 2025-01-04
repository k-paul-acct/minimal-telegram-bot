namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

/// <summary>
///     Represents the minimal entity to store states.
/// </summary>
public interface IMinimalTelegramBotState
{
    /// <summary>
    ///     Gets or sets the unique identifier for the user.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    ///     Gets or sets the unique identifier for the chat.
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    ///     Gets or sets the unique identifier for the message thread.
    /// </summary>
    public long MessageThreadId { get; set; }

    /// <summary>
    ///     Gets or sets the name of the state group.
    /// </summary>
    public string StateGroupName { get; set; }

    /// <summary>
    ///     Gets or sets the identifier of the state.
    /// </summary>
    public int StateId { get; set; }

    /// <summary>
    ///     Gets or sets the data associated with the state.
    /// </summary>
    public string StateData { get; set; }
}
