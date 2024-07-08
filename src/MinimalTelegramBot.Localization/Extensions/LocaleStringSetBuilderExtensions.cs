using System.Text.Json;
using System.Text.Json.Nodes;
using MinimalTelegramBot.Localization.Abstractions;
using YamlDotNet.Serialization;

namespace MinimalTelegramBot.Localization.Extensions;

public static class LocaleStringSetBuilderExtensions
{
    public static ILocaleStringSetBuilder EnrichFromJson(this ILocaleStringSetBuilder builder, string json)
    {
        var flattened = FlattenJson(json);
        return builder.Enrich(flattened.ToDictionary());
    }

    public static ILocaleStringSetBuilder EnrichFromYaml(this ILocaleStringSetBuilder builder, string yaml)
    {
        var flattened = FlattenYaml(yaml);
        return builder.Enrich(flattened.ToDictionary());
    }

    private static IEnumerable<(string, string)> FlattenJson(string json)
    {
        var node = JsonNode.Parse(json)!;
        return FlattenJsonRecursion(node);
    }

    private static IEnumerable<(string, string)> FlattenJsonRecursion(JsonNode node)
    {
        foreach (var (_, value) in node.AsObject())
        {
            if (value!.GetValueKind() == JsonValueKind.String)
            {
                yield return (value.GetPath()[2..], value.GetValue<string>());
                continue;
            }

            foreach (var pair in FlattenJsonRecursion(value))
            {
                yield return pair;
            }
        }
    }

    private static IEnumerable<(string, string)> FlattenYaml(string yaml)
    {
        var deserializer = new Deserializer();
        var yamlObject = deserializer.Deserialize(yaml);
        var jsonNode = JsonSerializer.SerializeToNode(yamlObject);
        return FlattenJsonRecursion(jsonNode!);
    }
}