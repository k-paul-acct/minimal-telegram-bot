namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

public interface IMinimalTelegramBotState
{
    public long UserId { get; set; }
    public string States { get; set; }
}
