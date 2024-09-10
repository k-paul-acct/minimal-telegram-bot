namespace MinimalTelegramBot.Handling;

public sealed class Handler
{
    private readonly Dictionary<Type, object[]> _metadata;

    public Handler(BotRequestDelegate requestDelegate, Dictionary<Type, object[]> metadata)
    {
        RequestDelegate = requestDelegate;
        _metadata = metadata;
    }

    public BotRequestDelegate RequestDelegate { get; }

    public bool SatisfyRequirements(ICollection<UpdateHandlingRequirement> requirements)
    {
        if (_metadata.Count == 0)
        {
            return true;
        }

        foreach (var requirement in requirements)
        {
            var requirementType = requirement.Requirement.GetType();

            if (!_metadata.TryGetValue(requirementType, out var objects))
            {
                return false;
            }

            if (requirementType == typeof(UpdateTypeAttribute))
            {
                var updateTypeAttribute = (UpdateTypeAttribute)requirement.Requirement;
                var pass = CheckUpdateTypeRequirement(objects, updateTypeAttribute);
                if (!pass)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool CheckUpdateTypeRequirement(object[] metadata, UpdateTypeAttribute requirement)
    {
        foreach (var obj in metadata)
        {
            var updateTypeMetadata = (UpdateTypeAttribute)obj;
            if (updateTypeMetadata.UpdateType == requirement.UpdateType)
            {
                return true;
            }
        }

        return false;
    }
}
