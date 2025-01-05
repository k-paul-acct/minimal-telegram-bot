using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

/// <summary>
///     The default implementation of <see cref="IMinimalTelegramBotState"/>.
/// </summary>
[PrimaryKey(nameof(UserId), nameof(ChatId), nameof(MessageThreadId))]
public class MinimalTelegramBotState : IMinimalTelegramBotState
{
    /// <inheritdoc/>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long UserId { get; set; }

    /// <inheritdoc/>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ChatId { get; set; }

    /// <inheritdoc/>
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long MessageThreadId { get; set; }

    /// <inheritdoc/>
    public string StateGroupName { get; set; } = null!;

    /// <inheritdoc/>
    public int StateId { get; set; }

    /// <inheritdoc/>
    public string StateData { get; set; } = null!;
}
