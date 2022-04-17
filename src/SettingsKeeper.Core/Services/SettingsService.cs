using Microsoft.AspNetCore.Http;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.Provider.Abstract;
using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Core.Services;

public class SettingsService: ISettingsService
{
    private readonly IRedisProvider _redisProvider;
    private readonly ISettingsProvider _settingsProvider;
    private readonly IHttpContextAccessor _context;
    private const string REDIS_CACHE_KEY = "settings_{0}";

    public SettingsService(IRedisProvider redisProvider, ISettingsProvider settingsProvider, IHttpContextAccessor context)
    {
        _redisProvider = redisProvider;
        _settingsProvider = settingsProvider;
        _context = context;
    }
    public async Task<string> GetSettingsAsync(string name, CancellationToken cancellationToken)
    {
        return await GetSettingsFromCacheAsync(name)
               ?? await GetSettingsFromMongoAsync(name, cancellationToken);
    }

    private async Task<string> GetSettingsFromMongoAsync(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        var collectionName = _context?.HttpContext?.Request?.Headers?.UserAgent;
        var result = await _settingsProvider.GetSettingsAsync(name, collectionName, cancellationToken);
        return result?.AppSettings;
    }

    private async Task<string> GetSettingsFromCacheAsync(string name)
    {
        return await _redisProvider.GetAsync<string>(string.Format(REDIS_CACHE_KEY, name));
    }
}