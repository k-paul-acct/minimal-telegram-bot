using System.Linq.Expressions;
using MinimalTelegramBot.Handling;

namespace MinimalTelegramBot.StateMachine;

internal sealed class StateParameterHandlerDelegateBuilderInterceptor : IHandlerDelegateBuilderInterceptor
{
    private readonly ReflectionStateSerializerContext _serializerContext;

    public StateParameterHandlerDelegateBuilderInterceptor(ReflectionStateSerializerContext serializerContext)
    {
        _serializerContext = serializerContext;
    }

    public Expression? CheckType(Type type, ParameterExpression context)
    {
        if (!_serializerContext.GetInfo(type, out _))
        {
            return null;
        }

        var data = Expression.Property(context, nameof(BotRequestContext.Data));
        var methodInfo = typeof(IDictionary<string, object?>).GetMethod(nameof(IDictionary<string, object?>.TryGetValue))!;
        var outVariable = Expression.Variable(typeof(object), "name");
        var call = Expression.Call(data, methodInfo, Expression.Constant("__State"), outVariable);
        var convert = Expression.Convert(call, type);
        return convert;
    }
}
