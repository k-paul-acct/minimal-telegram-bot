using MinimalTelegramBot.Handling.Requirements;

namespace MinimalTelegramBot.Handling;

public sealed class Handler
{
    public Handler(BotRequestDelegate filteredBotRequestDelegate, Dictionary<Type, object[]> metadata)
    {
        FilteredBotRequestDelegate = filteredBotRequestDelegate;
        Metadata = metadata;
    }

    public Dictionary<Type, object[]> Metadata { get; }
    public BotRequestDelegate FilteredBotRequestDelegate { get; }

    public bool SatisfyRequirements(IReadOnlyCollection<UpdateHandlingRequirement> requirements)
    {
        if (Metadata.Count == 0)
        {
            return true;
        }

        foreach (var requirement in requirements)
        {
            // TODO:
            var requirementType = requirement.Requirement.GetType();

            if (!Metadata.TryGetValue(requirementType, out var objects))
            {
                return false;
            }

            if (requirementType == typeof(UpdateTypeRequirement))
            {
                var updateTypeAttribute = (UpdateTypeRequirement)requirement.Requirement;
                var pass = CheckUpdateTypeRequirement(objects, updateTypeAttribute);
                if (!pass)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool CheckUpdateTypeRequirement(object[] metadata, UpdateTypeRequirement requirement)
    {
        foreach (var obj in metadata)
        {
            var updateTypeMetadata = (UpdateTypeRequirement)obj;
            if (updateTypeMetadata.UpdateType == requirement.UpdateType)
            {
                return true;
            }
        }

        return false;
    }
}
