using System.Linq.Expressions;

namespace MinimalTelegramBot.Handling;

/// <summary>
///     Interceptor for handler delegate builder.
/// </summary>
public interface IHandlerDelegateBuilderInterceptor
{
    /// <summary>
    ///     Check if the type is supported by the interceptor.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="context">The <see cref="BotRequestContext"/> parameter expression.</param>
    /// <returns>
    ///     An expression to retrieve parameter with the specified type from the <see cref="BotRequestContext"/>
    ///     if the type is supported, otherwise null.
    /// </returns>
    Expression? CheckType(Type type, ParameterExpression context);
}
