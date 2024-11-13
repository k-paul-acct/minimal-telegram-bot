namespace MinimalTelegramBot.Handling;

/// <summary>
///     Options for building handler delegate.
/// </summary>
public sealed class HandlerDelegateBuilderOptions
{
    /// <summary>
    ///     Gets the collection of interceptors that used to resolve the parameters available for substitution into the handler delegate.
    /// </summary>
    public ICollection<IHandlerDelegateBuilderInterceptor> Interceptors { get; } = [];
}
