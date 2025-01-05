using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MinimalTelegramBot.Localization.Extensions;

/// <summary>
///     ServiceCollectionExtensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds localization services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="build">An action to configure the <see cref="ILocaleStringSetRepositoryBuilder"/>.</param>
    /// <typeparam name="TUserLocaleProvider">The type of the user locale provider.</typeparam>
    /// <returns>The current instance of <see cref="IServiceCollection"/>.</returns>
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

    /// <summary>
    ///     Adds a single <see cref="Locale"/> to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="locale">The <see cref="Locale"/> to be added.</param>
    /// <param name="build">An action to configure the <see cref="ILocaleStringSetBuilder"/>.</param>
    /// <returns>The current instance of <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSingleLocale(this IServiceCollection services, Locale locale, Action<ILocaleStringSetBuilder> build)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(locale);
        ArgumentNullException.ThrowIfNull(build);

        Locale.Default = locale;

        var builder = new LocaleStringSetBuilder(locale);
        build(builder);
        var set = builder.Build();
        var repository = new InMemoryLocaleStringSetRepository([set,]);

        services.TryAddSingleton<ILocalizer, Localizer>();
        services.TryAddSingleton<ILocaleStringSetRepository>(repository);
        services.TryAddSingleton<IUserLocaleProvider>(new SingleLocaleUserLocaleProvider(locale));

        return services;
    }
}
