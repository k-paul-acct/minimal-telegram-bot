using System.Text.Json;
using System.Text.Json.Nodes;
using YamlDotNet.Serialization;

namespace MinimalTelegramBot.Localization.Extensions;

public static class LocaleStringSetBuilderExtensions
{
    public static ILocaleStringSetBuilder EnrichFromJson(this ILocaleStringSetBuilder builder, string json)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(json);

        var flattened = FlattenJson(json);
        return builder.Enrich(flattened.ToDictionary());
    }

    public static ILocaleStringSetBuilder EnrichFromYaml(this ILocaleStringSetBuilder builder, string yaml)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(yaml);

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

    public static ILocaleStringSetBuilder EnrichFromFile(this ILocaleStringSetBuilder builder, string fileName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(fileName);

        var content = File.ReadAllText(fileName);
        var extension = Path.GetExtension(fileName).ToLower() ?? throw new Exception("File must have an extension");

        switch (extension)
        {
            case ".json":
                builder.EnrichFromJson(content);
                break;
            case ".yaml" or ".yml":
                builder.EnrichFromYaml(content);
                break;
            default:
                throw new NotSupportedException($"{extension} file extension not supported");
        }

        return builder;
    }

    private static IEnumerable<(string, string)> FlattenYaml(string yaml)
    {
        var deserializer = new Deserializer();
        var yamlObject = deserializer.Deserialize(yaml);
        var jsonNode = JsonSerializer.SerializeToNode(yamlObject);
        return FlattenJsonRecursion(jsonNode!);
    }
}