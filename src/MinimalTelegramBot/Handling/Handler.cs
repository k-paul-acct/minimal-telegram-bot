using MinimalTelegramBot.Handling.Requirements;

namespace MinimalTelegramBot.Handling;

/// <summary>
///     Represents a handler for processing bot request.
/// </summary>
public sealed class Handler
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Handler"/>.
    /// </summary>
    /// <param name="filteredBotRequestDelegate">The delegate with the filter pipeline applied to handle bot request.</param>
    /// <param name="metadata">The metadata associated with the handler.</param>
    public Handler(BotRequestDelegate filteredBotRequestDelegate, Dictionary<Type, object[]> metadata)
    {
        FilteredBotRequestDelegate = filteredBotRequestDelegate;
        Metadata = metadata;
    }

    /// <summary>
    ///     Gets the metadata associated with the handler.
    /// </summary>
    public Dictionary<Type, object[]> Metadata { get; }

    /// <summary>
    ///     Gets the handler delegate with the filter pipeline applied.
    /// </summary>
    public BotRequestDelegate FilteredBotRequestDelegate { get; }

    /// <summary>
    ///     Checks if the handler satisfies the given requirements.
    /// </summary>
    /// <param name="requirements">A collection of requirements to be satisfied.</param>
    /// <returns>True if all requirements are satisfied, otherwise, false.</returns>
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
