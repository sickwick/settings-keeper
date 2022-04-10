using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Cache.Models;
using SettingsKeeper.Cache.Providers;
using StackExchange.Redis;

namespace SettingsKeeper.Cache.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSettingsKeeperCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(configuration.GetSection("Redis"));
        services.AddSingleton((provider) =>
        {
            var options = provider.GetRequiredService<IOptions<RedisSettings>>();
            return CreateRedisConnector(options);
        });
        services.AddSingleton<ISettingsKeeperCacheProvider, SettingsKeeperCacheProvider>();
        return services;
    }

    private static IDatabase CreateRedisConnector(IOptions<RedisSettings> redisSettings)
    {
        var connection = redisSettings?.Value?.ConnectionString 
                         ?? throw new ArgumentNullException(nameof(redisSettings));
        var redis = ConnectionMultiplexer.Connect(connection);
        var db = redis.GetDatabase();

        return db;
    }
}