namespace MinimalTelegramBot.Handling;

internal static class AwaitableInfo
{
    public static bool IsTypeAwaitable(Type type, out Type? taskType, out Type? genericType)
    {
        taskType = null;
        genericType = null;

        if (type == typeof(Task))
        {
            taskType = typeof(Task);
            return true;
        }

        if (type == typeof(ValueTask))
        {
            taskType = typeof(ValueTask);
            return true;
        }

        if (!type.IsGenericType)
        {
            return false;
        }

        var genericDefinition = type.GetGenericTypeDefinition();

        if (genericDefinition == typeof(Task<>))
        {
            taskType = typeof(Task);
            genericType = type.GenericTypeArguments[0];
            return true;
        }

        if (genericDefinition != typeof(ValueTask<>))
        {
            return false;
        }

        taskType = typeof(ValueTask);
        genericType = type.GenericTypeArguments[0];

        return true;
    }
}
