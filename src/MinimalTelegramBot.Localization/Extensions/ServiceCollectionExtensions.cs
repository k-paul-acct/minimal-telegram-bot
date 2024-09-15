using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MinimalTelegramBot.Localization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalizer<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TUserLocaleProvider>(this IServiceCollection services, Action<ILocaleStringSetRepositoryBuilder> build)
        where TUserLocaleProvider : class, IUserLocaleProvider
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(build);

        var builder = new LocaleStringSetRepositoryBuilder();
        build(builder);
        var repository = builder.Build();

        services.TryAddSingleton(repository);
        services.TryAddSingleton<ILocalizer, Localizer>();
        services.TryAddScoped<IUserLocaleProvider, TUserLocaleProvider>();

        return services;
    }

    public static IServiceCollection AddSingleLocale(this IServiceCollection services, Locale locale, Action<ILocaleStringSetBuilder> build)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(locale);
        ArgumentNullException.ThrowIfNull(build);

        Locale.Default = locale;

        var builder = new LocaleStringSetBuilder(locale);
        build(builder);
        var set = builder.Build();
        var repository = new InMemoryLocaleStringSetRepository();
        repository.AddLocaleStringSet(set);

        services.TryAddSingleton<ILocalizer, Localizer>();
        services.TryAddSingleton<ILocaleStringSetRepository>(repository);
        services.TryAddSingleton<IUserLocaleProvider>(new SingleLocaleUserLocaleProvider(locale));

        return services;
    }
}
