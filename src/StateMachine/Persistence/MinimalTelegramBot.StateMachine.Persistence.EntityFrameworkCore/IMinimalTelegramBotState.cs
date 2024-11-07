namespace MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

public interface IMinimalTelegramBotState
{
    public long UserId { get; set; }
    public string StateGroupName { get; set; }
    public int StateId { get; set; }
    public string StateData { get; set; }
}
