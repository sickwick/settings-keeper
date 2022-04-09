namespace SettingsKeeper.Cache.Abstract;

public interface ISettingsKeeperCacheProvider
{
    Task<string> GetAsync(string cacheKey, CancellationToken cancellationToken);

    Task SetAsync<T>(string cacheKey, T data, CancellationToken cancellationToken) where T: class;

    Task RemoveAsync(string cacheKey, CancellationToken cancellationToken);
}