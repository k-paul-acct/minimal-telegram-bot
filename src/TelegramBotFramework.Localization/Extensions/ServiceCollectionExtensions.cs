using Microsoft.Extensions.DependencyInjection;
using TelegramBotFramework.Abstractions;
using TelegramBotFramework.Localization.Abstractions;
using TelegramBotFramework.Localization.Abstractions.Providers;

namespace TelegramBotFramework.Localization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalizer<TUserId, TUserIdProvider, TUserLocaleProvider>(
        this IServiceCollection services, ILocaleStringSetRepository repository)
        where TUserId : notnull
        where TUserIdProvider : class, IUserIdProvider<TUserId>
        where TUserLocaleProvider : class, IUserLocaleProvider<TUserId>
    {
        services.AddSingleton(repository);
        services.AddSingleton<IUserLocaleRepository<TUserId>, InMemoryUserLocaleRepository<TUserId>>();
        services.AddScoped<ILocalizer, Localizer<TUserId>>();
        services.AddScoped<IUserIdProvider<TUserId>, TUserIdProvider>();
        services.AddScoped<IUserLocaleProvider<TUserId>, TUserLocaleProvider>();
        services.AddScoped<IUserLocaleService<TUserId>, UserLocaleService<TUserId>>();
        return services;
    }
}