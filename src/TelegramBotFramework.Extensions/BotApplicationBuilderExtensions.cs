using System.Text;
using TelegramBotFramework.Localization;
using TelegramBotFramework.Localization.Abstractions;
using TelegramBotFramework.Localization.Abstractions.Providers;
using TelegramBotFramework.Localization.Extensions;
using TelegramBotFramework.Services;

namespace TelegramBotFramework.Extensions;

public static class BotApplicationBuilderExtensions
{
    public static BotApplicationBuilder AddLocalizer<TUserLocaleProvider>(this BotApplicationBuilder builder,
        IEnumerable<string> languageCodes) where TUserLocaleProvider : class, IUserLocaleProvider<long>
    {
        var repository = new InMemoryLocaleStringSetRepository();
        foreach (var languageCode in languageCodes)
        {
            var content = File.ReadAllText($"Prefabs/Languages/{languageCode}.yaml", Encoding.UTF8);
            var set = new LocaleStringSetBuilder(new Locale(languageCode)).EnrichFromYaml(content).Build();
            repository.AddLocaleStringSet(set);
        }
        builder.HostBuilder.Services.AddLocalizer<long, TelegramUserIdProvider, TUserLocaleProvider>(repository);
        return builder;
    }
}