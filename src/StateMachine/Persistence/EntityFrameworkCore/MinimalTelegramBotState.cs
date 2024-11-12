using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

// TODO: Docs.
[PrimaryKey(nameof(UserId), nameof(ChatId), nameof(MessageThreadId))]
public class MinimalTelegramBotState : IMinimalTelegramBotState
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long UserId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ChatId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long MessageThreadId { get; set; }

    public string StateGroupName { get; set; } = null!;
    public int StateId { get; set; }
    public string StateData { get; set; } = null!;
}
