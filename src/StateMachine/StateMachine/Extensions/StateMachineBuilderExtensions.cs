namespace MinimalTelegramBot.StateMachine.Extensions;

public static class StateMachineBuilderExtensions
{
    public static IStateMachineBuilder UseRepository<TRepository>(this IStateMachineBuilder builder)
        where TRepository : IUserStateRepository
    {
        return builder;
    }
}
