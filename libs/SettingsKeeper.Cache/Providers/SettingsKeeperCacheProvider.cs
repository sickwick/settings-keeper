using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Newtonsoft.Json;
using SettingsKeeper.Cache.Abstract;
using StackExchange.Redis;

namespace SettingsKeeper.Cache.Providers;

public class SettingsKeeperCacheProvider: ISettingsKeeperCacheProvider
{
    private readonly IDatabase _database;

    public SettingsKeeperCacheProvider(IDatabase database)
    {
        _database = database;
    }
    public async Task<string> GetAsync(string cacheKey, CancellationToken cancellationToken)
    {
        return await _database.StringGetAsync(cacheKey);
    }

    public async Task SetAsync<T>(string cacheKey, T data, CancellationToken cancellationToken)
    where T: class
    {
        var stringData = JsonConvert.SerializeObject(data);
        var resultData = Encoding.UTF8.GetBytes(stringData);
        await _database.StringSetAsync(cacheKey, resultData);
    }

    public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}