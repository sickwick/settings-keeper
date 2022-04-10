namespace SettingsKeeper.Cache.Abstract;

public interface IRedisProvider
{
    Task<T> GetAsync<T>(string cacheKey);

    Task SetAsync<T>(string cacheKey, T data) where T: class;

    Task<bool> RemoveAsync(string cacheKey);
}