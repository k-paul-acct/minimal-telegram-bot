using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MinimalTelegramBot.StateMachine;

internal sealed class ReflectionStateTypeInfoResolver : IStateTypeInfoResolver
{
    private readonly FrozenDictionary<Type, StateEntry> _stateInfo;
    private readonly FrozenDictionary<StateEntry, Type> _stateInfoReverse;

    public ReflectionStateTypeInfoResolver()
    {
        var stateInfo = new List<KeyValuePair<Type, StateEntry>>();
        var stateInfoReverse = new List<KeyValuePair<StateEntry, Type>>();
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

                var info = new StateEntry(stateGroupAttribute.StateGroupName, stateAttribute.StateId, "{}");

                stateInfo.Add(new KeyValuePair<Type, StateEntry>(nestedType, info));
                stateInfoReverse.Add(new KeyValuePair<StateEntry, Type>(info, nestedType));
            }
        }

        _stateInfo = stateInfo.ToFrozenDictionary();
        _stateInfoReverse = stateInfoReverse.ToFrozenDictionary();
    }

    public bool GetInfo(Type type, out StateEntry stateEntry)
    {
        return _stateInfo.TryGetValue(type, out stateEntry);
    }

    public bool GetInfo(StateEntry stateEntry, [NotNullWhen(true)] out Type? stateType)
    {
        return _stateInfoReverse.TryGetValue(stateEntry with { StateData = "{}", }, out stateType);
    }
}
