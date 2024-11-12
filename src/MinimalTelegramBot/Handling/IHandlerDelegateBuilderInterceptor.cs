using System.Linq.Expressions;

namespace MinimalTelegramBot.Handling;

// TODO: Docs.
/// <summary>
/// </summary>
public interface IHandlerDelegateBuilderInterceptor
{
    /// <summary>
    /// </summary>
    /// <param name="type"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    Expression? CheckType(Type type, ParameterExpression context);
}
