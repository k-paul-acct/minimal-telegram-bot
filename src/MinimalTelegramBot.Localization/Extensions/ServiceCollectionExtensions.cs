using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalTelegramBot.Localization.Abstractions;
using MinimalTelegramBot.Localization.Abstractions.Providers;

namespace MinimalTelegramBot.Localization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalizer<TUserLocaleProvider>(this IServiceCollection services,
        Action<ILocalizerBuilder> localizerBuild)
        where TUserLocaleProvider : class, IUserLocaleProvider
    {
        services.TryAddSingleton<IUserLocaleRepository, InMemoryUserLocaleRepository>();
        services.TryAddScoped<IUserLocaleProvider, TUserLocaleProvider>();
        services.TryAddScoped<ILocalizer, Localizer>();
        services.TryAddScoped<IUserLocaleService, UserLocaleService>();

        var builder = new LocalizerBuilder();
        localizerBuild(builder);
        var repository = builder.Build();
        services.AddSingleton(repository);

        return services;
    }
}