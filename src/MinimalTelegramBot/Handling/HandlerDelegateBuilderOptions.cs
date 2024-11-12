namespace MinimalTelegramBot.Handling;

// TODO: Docs.
/// <summary>
/// </summary>
public sealed class HandlerDelegateBuilderOptions
{
    /// <summary>
    /// </summary>
    public ICollection<IHandlerDelegateBuilderInterceptor> Interceptors { get; } = [];
}
