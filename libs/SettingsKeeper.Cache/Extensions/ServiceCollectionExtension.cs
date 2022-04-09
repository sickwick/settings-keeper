using Microsoft.Extensions.DependencyInjection;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Cache.Providers;
using StackExchange.Redis;

namespace SettingsKeeper.Cache.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSettingsKeeperCache(this IServiceCollection services)
    {
        services.AddSingleton(CreateRedisConnector());
        services.AddSingleton<ISettingsKeeperCacheProvider, SettingsKeeperCacheProvider>();
        return services;
    }

    private static IDatabase CreateRedisConnector()
    {
        var redis = ConnectionMultiplexer.Connect("localhost:6379");
        var db = redis.GetDatabase();

        return db;
    }
}