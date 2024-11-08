using System.Text.Json;
using System.Text.Json.Nodes;
using YamlDotNet.Serialization;

namespace MinimalTelegramBot.Localization.Extensions;

/// <summary>
///     LocaleStringSetBuilderExtensions.
/// </summary>
public static class LocaleStringSetBuilderExtensions
{
    /// <summary>
    ///     Enriches the <see cref="ILocaleStringSetBuilder"/> with locale strings from a JSON string.
    /// </summary>
    /// <param name="builder">The <see cref="ILocaleStringSetBuilder"/> to be enriched.</param>
    /// <param name="json">The JSON string containing locale strings.</param>
    /// <returns>The current instance of <see cref="ILocaleStringSetBuilder"/>.</returns>
    public static ILocaleStringSetBuilder EnrichFromJson(this ILocaleStringSetBuilder builder, string json)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(json);

        var flattened = FlattenJson(json);
        return builder.Enrich(flattened.ToDictionary());
    }

    /// <summary>
    ///     Enriches the <see cref="ILocaleStringSetBuilder"/> with locale strings from a YAML string.
    /// </summary>
    /// <param name="builder">The <see cref="ILocaleStringSetBuilder"/> to be enriched.</param>
    /// <param name="yaml">The YAML string containing locale strings.</param>
    /// <returns>The current instance of <see cref="ILocaleStringSetBuilder"/>.</returns>
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

    /// <summary>
    ///     Enriches the <see cref="ILocaleStringSetBuilder"/> with locale strings from a file.
    /// </summary>
    /// <param name="builder">The <see cref="ILocaleStringSetBuilder"/> to be enriched.</param>
    /// <param name="fileName">The name of the file containing locale strings.</param>
    /// <returns>The current instance of <see cref="ILocaleStringSetBuilder"/>.</returns>
    /// <exception cref="ArgumentException">The given file name has no extension.</exception>
    /// <exception cref="NotSupportedException">
    ///     The extension of the given file name not supported.
    ///     Extensions for JSON and YAML files are currently supported.
    /// </exception>
    public static ILocaleStringSetBuilder EnrichFromFile(this ILocaleStringSetBuilder builder, string fileName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(fileName);

        var content = File.ReadAllText(fileName);
        var extension = Path.GetExtension(fileName).ToLower() ?? throw new ArgumentException("File must have an extension", nameof(fileName));

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
