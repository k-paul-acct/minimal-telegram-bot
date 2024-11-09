using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

[Index(nameof(UserId), IsUnique = true)]
public class MinimalTelegramBotState : IMinimalTelegramBotState
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long UserId { get; set; }

    public string States { get; set; } = "[]";
}
