using Microsoft.Extensions.DependencyInjection;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.Core.Services;

namespace SettingsKeeper.Config;

public static class ContainerConfig
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IFeatureToogleService, FeatureToogleService>();
        services.AddSingleton<ISettingsService, SettingsService>();

        return services;
    }
}