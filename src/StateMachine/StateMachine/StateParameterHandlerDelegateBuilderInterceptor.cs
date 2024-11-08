using System.Linq.Expressions;
using MinimalTelegramBot.Handling;

namespace MinimalTelegramBot.StateMachine;

internal sealed class StateParameterHandlerDelegateBuilderInterceptor : IHandlerDelegateBuilderInterceptor
{
    private readonly IStateTypeInfoResolver _typeInfoResolver;

    public StateParameterHandlerDelegateBuilderInterceptor(IStateTypeInfoResolver typeInfoResolver)
    {
        _typeInfoResolver = typeInfoResolver;
    }

    public Expression? CheckType(Type type, ParameterExpression context)
    {
        if (!_typeInfoResolver.GetInfo(type, out _))
        {
            return null;
        }

        var data = Expression.Property(context, nameof(BotRequestContext.Data));
        var methodInfo = typeof(IDictionary<string, object?>).GetMethod(nameof(IDictionary<string, object?>.TryGetValue))!;
        var outVariable = Expression.Variable(typeof(object), "state");
        var call = Expression.Call(data, methodInfo, Expression.Constant("__State"), outVariable);
        var convert = Expression.Convert(outVariable, type);
        return Expression.Block(type, [outVariable,], call, convert);
    }
}
