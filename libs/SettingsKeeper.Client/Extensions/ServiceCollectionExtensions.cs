using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SettingsKeeper.Client.Services;
using SettingsKeeper.RabbitMQ.Abstract;
using SettingsKeeper.RabbitMQ.Extensions;
using SettingsKeeper.RabbitMQ.Services;

namespace SettingsKeeper.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSettingsKeeperClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMqForClient(configuration);
        services.AddSingleton<IRabbitMqResult, SettingsKeeperService>();
        services.AddHostedService<RabbitMqListener>();
    }
}