using System.Runtime.InteropServices;
using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.Localization.Extensions;

namespace MinimalTelegramBot.Localization;

public class LocalizerBuilder : ILocalizerBuilder
{
    private readonly InMemoryLocaleStringSetRepository _repository = new();
    private readonly Dictionary<Locale, ILocaleStringSetBuilder> _setBuilders = new();

    public ILocalizerBuilder EnrichFromFile(string fileName, Locale locale)
    {
        ref var setBuilder = ref CollectionsMarshal.GetValueRefOrAddDefault(_setBuilders, locale, out var exists);
        if (!exists)
        {
            setBuilder = new LocaleStringSetBuilder(locale);
        }

        var content = File.ReadAllText(fileName);
        var extension = Path.GetExtension(fileName) ?? throw new Exception("File must have an extension");

        switch (extension.ToLower())
        {
            case ".json":
                setBuilder!.EnrichFromJson(content);
                break;
            case ".yaml" or ".yml":
                setBuilder!.EnrichFromYaml(content);
                break;
            default:
                throw new NotSupportedException($"{extension} file extension not supported");
        }

        return this;
    }

    public ILocaleStringSetRepository Build()
    {
        var sets = _setBuilders.Select(x => x.Value.Build());

        foreach (var set in sets)
        {
            _repository.AddLocaleStringSet(set);
        }

        return _repository;
    }
}