// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.MongoDb.Models;
using SettingsKeeper.MongoDb.Providers;



namespace SettingsKeeper.MongoDb.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddOptions<MongoSettings>()
        //     .Bind(configuration.GetSection("MongoDb"))
        //     .ValidateDataAnnotations();
        services.Configure<MongoSettings>(configuration.GetSection("MongoDb"));
        services.AddSingleton<IMongoDbProvider, MongoDbProvider>();
        return services;
    }
}