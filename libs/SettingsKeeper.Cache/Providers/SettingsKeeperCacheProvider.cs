using System.Text;
using Newtonsoft.Json;
using SettingsKeeper.Cache.Abstract;
using StackExchange.Redis;

namespace SettingsKeeper.Cache.Providers;

public class SettingsKeeperCacheProvider: ISettingsKeeperCacheProvider
{
    private readonly IDatabase _database;
    private const int DefaultCacheLifeTime = 8;

    public SettingsKeeperCacheProvider(IDatabase database)
    {
        _database = database;
    }
    public async Task<string> GetAsync(string cacheKey)
    {
        return await _database.StringGetAsync(cacheKey);
    }

    public async Task SetAsync<T>(string cacheKey, T data)
    where T: class
    {
        var stringData = JsonConvert.SerializeObject(data);
        var resultData = Encoding.UTF8.GetBytes(stringData);
        await _database.StringSetAsync(cacheKey, resultData, TimeSpan.FromHours(DefaultCacheLifeTime));
    }

    public async Task<bool> RemoveAsync(string cacheKey)
    {
        return await _database.KeyDeleteAsync(cacheKey);
    }
}