using System.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using SettingsKeeper.Client.Abstraction;
using SettingsKeeper.Client.Constants;
using SettingsKeeper.Client.Models;
using SettingsKeeper.Client.Providers;
using SettingsKeeper.Client.Services;
using SettingsKeeper.RabbitMQ.Abstract;
using SettingsKeeper.RabbitMQ.Extensions;
using SettingsKeeper.RabbitMQ.Models;
using SettingsKeeper.RabbitMQ.Services;

namespace SettingsKeeper.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSettingsKeeperClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMqForClient(configuration);
        services.AddSingleton<IRabbitMqResult, SettingsKeeperService>();
        services.Configure<ClientOptions>(configuration.GetSection("Keeper:SettingsKeeperClient"));
        services.Configure<RabbitClientOptions>(configuration.GetSection("Keeper:RabbitClient"));

        services.AddHttpClient();
        
        services.AddHostedService<AppRegistratorHostedService>();
        services.AddHostedService<RabbitMqListener>();

        services.AddFeatureManagement();
        services.AddSingleton<IFeatureDefinitionProvider, FeatureDefinitionProvider>();
        services.AddSingleton<IFeatureToogleService, FeatureToogleService>();
    }

    private static void AddHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(ClientConstants.HttpClient, (serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ClientOptions>>();
            var media = new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json);
            var acceptCharset = new StringWithQualityHeaderValue("UTF-8");

            client.BaseAddress = new Uri(options?.Value?.BaseUrl);
            client.DefaultRequestHeaders.Add("User-Agent", "SettingsKeeper.Client");
            client.DefaultRequestHeaders.Accept.Add(media);
            client.DefaultRequestHeaders.AcceptCharset.Add(acceptCharset);
        });
    }
}