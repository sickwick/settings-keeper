using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SettingsKeeper.RabbitMQ.Abstract;
using SettingsKeeper.RabbitMQ.Models;
using SettingsKeeper.RabbitMQ.Services;

namespace SettingsKeeper.RabbitMQ.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitOptions>(configuration.GetSection("Rabbit"));
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
    }
}