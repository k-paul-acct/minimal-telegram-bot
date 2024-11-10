using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MinimalTelegramBot.StateMachine.Abstractions;

namespace MinimalTelegramBot.StateMachine;

internal sealed class ReflectionStateTypeInfoResolver : IStateTypeInfoResolver
{
    private readonly FrozenDictionary<Type, StateTypeInfo> _stateInfo;
    private readonly FrozenDictionary<StateTypeInfo, Type> _stateInfoReverse;

    public ReflectionStateTypeInfoResolver()
    {
        var stateInfo = new List<KeyValuePair<Type, StateTypeInfo>>();
        var stateInfoReverse = new List<KeyValuePair<StateTypeInfo, Type>>();
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

                var info = new StateTypeInfo(stateGroupAttribute.StateGroupName, stateAttribute.StateId);

                stateInfo.Add(new KeyValuePair<Type, StateTypeInfo>(nestedType, info));
                stateInfoReverse.Add(new KeyValuePair<StateTypeInfo, Type>(info, nestedType));
            }
        }

        _stateInfo = stateInfo.ToFrozenDictionary();
        _stateInfoReverse = stateInfoReverse.ToFrozenDictionary();
    }

    public bool GetInfo(Type type, out StateTypeInfo typeInfo)
    {
        return _stateInfo.TryGetValue(type, out typeInfo);
    }

    public bool GetInfo(StateTypeInfo typeInfo, [NotNullWhen(true)] out Type? stateType)
    {
        return _stateInfoReverse.TryGetValue(typeInfo, out stateType);
    }
}
