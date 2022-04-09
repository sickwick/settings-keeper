namespace SettingsKeeper.Cache.Abstract;

public interface ISettingsKeeperCacheProvider
{
    Task<string> GetAsync(string cacheKey);

    Task SetAsync<T>(string cacheKey, T data) where T: class;

    Task<bool> RemoveAsync(string cacheKey);
}