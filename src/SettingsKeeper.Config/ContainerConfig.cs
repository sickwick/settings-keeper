using Microsoft.Extensions.DependencyInjection;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.Core.Services;
using SettingsKeeper.Provider.Abstract;
using SettingsKeeper.Provider.Providers;

namespace SettingsKeeper.Config;

public static class ContainerConfig
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IFeatureToggleService, FeatureToggleService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IFeatureToogleProvider, FeatureToogleProvider>();
        services.AddSingleton<IClientsService, ClientsService>();

        services.AddSingleton<ISettingsProvider, SettingsProvider>();

        return services;
    }
}