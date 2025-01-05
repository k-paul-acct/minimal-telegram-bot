using Microsoft.EntityFrameworkCore;
using MinimalTelegramBot.StateMachine.Persistence.EntityFrameworkCore;

namespace UsageExample.StateMachine.EntityFrameworkCore;

public class AppDbContext : DbContext, IStateMachineDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<MinimalTelegramBotState> MinimalTelegramBotStates { get; set; }
}
