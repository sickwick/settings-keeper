using System.Text;
using Newtonsoft.Json;
using SettingsKeeper.Cache.Abstract;
using StackExchange.Redis;

namespace SettingsKeeper.Cache.Providers;

public class SettingsKeeperCacheProvider: IRedisProvider
{
    private readonly IDatabase _database;
    private const int DefaultCacheLifeTime = 8;

    public SettingsKeeperCacheProvider(IDatabase database)
    {
        _database = database;
    }
    public async Task<T> GetAsync<T>(string cacheKey)
    {
        var data = await _database.StringGetAsync(cacheKey);
        var encodedData = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(data);
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