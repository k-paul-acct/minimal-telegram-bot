using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;

namespace MinimalTelegramBot.StateMachine;

internal sealed class ReflectionStateSerializerContext : IStateSerializerContext
{
    private readonly Dictionary<Type, StateEntry> _stateInfo = [];
    private readonly Dictionary<StateEntry, Type> _stateInfoReverse = [];

    public ReflectionStateSerializerContext(JsonSerializerOptions? jsonSerializerOptions)
    {
        JsonSerializerOptions = jsonSerializerOptions;

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName is not null && !a.FullName.StartsWith("System.") && !a.FullName.StartsWith("Microsoft."));

        foreach (var groupType in assemblies.SelectMany(x => x.GetTypes()))
        {
            var stateGroupAttribute = groupType.GetCustomAttribute<StateGroupAttribute>(false);
            if (stateGroupAttribute is null)
            {
                continue;
            }

            foreach (var nestedType in groupType.GetNestedTypes(BindingFlags.Public))
            {
                if (nestedType.IsAbstract)
                {
                    continue;
                }

                var stateAttribute = nestedType.GetCustomAttribute<StateAttribute>(false);
                if (stateAttribute is null)
                {
                    continue;
                }

                var info = new StateEntry(stateGroupAttribute.StateGroupName, stateAttribute.StateId);

                _stateInfo.Add(nestedType, info);
                _stateInfoReverse.Add(info, nestedType);
            }
        }
    }

    public JsonSerializerOptions? JsonSerializerOptions { get; }

    public bool GetInfo(Type type, out StateEntry stateEntry)
    {
        return _stateInfo.TryGetValue(type, out stateEntry);
    }

    public bool GetInfo(StateEntry stateEntry, [NotNullWhen(true)] out Type? stateType)
    {
        return _stateInfoReverse.TryGetValue(stateEntry, out stateType);
    }
}
