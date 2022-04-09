using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace SettingsKeeper.Cache.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSettingsKeeperCache(this IServiceCollection services)
    {
        services.AddSingleton(CreateRedisConnector());
        return services;
    }

    private static IDatabase CreateRedisConnector()
    {
        var redis = ConnectionMultiplexer.Connect("localhost");
        var db = redis.GetDatabase();

        return db;
    }
}