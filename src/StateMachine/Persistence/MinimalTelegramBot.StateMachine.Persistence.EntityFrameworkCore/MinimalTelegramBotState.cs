using System.ComponentModel.DataAnnotations;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

public class MinimalTelegramBotState : IMinimalTelegramBotState
{
    [Key]
    public long UserId { get; set; }

    public string StateGroupName { get; set; } = null!;
    public int StateId { get; set; }
    public string StateData { get; set; } = null!;
}
