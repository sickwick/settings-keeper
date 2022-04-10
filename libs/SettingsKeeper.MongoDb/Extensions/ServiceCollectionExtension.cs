using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.MongoDb.Models;
using SettingsKeeper.MongoDb.Providers;

namespace SettingsKeeper.MongoDb.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MongoSettings>("MongoDb");
        services.AddSingleton<IMongoDbProvider, MongoDbProvider>();
        return services;
    }
}