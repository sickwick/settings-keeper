using System.Text.Json;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.Provider.Abstract;
using SettingsKeeper.Provider.Models;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.Core.Services;

public class SettingsService: ISettingsService
{
    private readonly IRedisProvider _redisProvider;
    private readonly ISettingsProvider _settingsProvider;
    private readonly IClientsService _clientsService;
    private readonly SettingsOptions _options;
    private const string REDIS_CACHE_KEY = "settings_{0}";
    private const int REDIS_LIFETIME = 4;

    public SettingsService(IRedisProvider redisProvider, 
        ISettingsProvider settingsProvider, 
        IOptions<SettingsKeeperSettings> options,
        IClientsService clientsService)
    {
        _redisProvider = redisProvider;
        _settingsProvider = settingsProvider;
        _clientsService = clientsService;
        _options = options?.Value?.SettingsOptions
                   ?? throw new ArgumentNullException(nameof(options));
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
        var collectionName = _options.CollectionName;
        if(string.IsNullOrEmpty(collectionName))
            throw new ArgumentNullException(nameof(collectionName), "Необходимо указать название коллекции");
        
        var result = await _settingsProvider.GetSettingsAsync(name, collectionName, cancellationToken);

        await _redisProvider.SetAsync(string.Format(REDIS_CACHE_KEY, name), result?.AppSettings, REDIS_LIFETIME);
        return result?.AppSettings;
    }

    private async Task<string?> GetSettingsFromCacheAsync(string name)
    {
        var result = await _redisProvider.GetAsync<string>(string.Format(REDIS_CACHE_KEY, name));
        return string.IsNullOrEmpty(result)? default: result;
    }

    public async Task AddSettingsAsync(string name, string settings, CancellationToken cancellationToken)
    {
        var request = new Settings() { Name = name, AppSettings = settings};
        var collectionName = _options.CollectionName;
        if(string.IsNullOrEmpty(collectionName))
            throw new ArgumentNullException(nameof(collectionName), "Необходимо указать название коллекции");
        await _settingsProvider.AddSettingsAsync(collectionName, request, cancellationToken);
    }

    public async Task ChangeSettingsAsync(string name, string settings, CancellationToken cancellationToken)
    {
        var request = new Settings() { Name = name, AppSettings = settings };
        var collectionName = _options.CollectionName;
        if(string.IsNullOrEmpty(collectionName))
            throw new ArgumentNullException(nameof(collectionName), "Необходимо указать название коллекции");
        await _settingsProvider.ChangeSettingsAsync(collectionName, name, request, cancellationToken);

        await _redisProvider.RemoveAsync(string.Format(REDIS_CACHE_KEY, name));
    }

    public async Task RemoveSettingsAsync(string name, CancellationToken cancellationToken)
    {
        var collectionName = _options.CollectionName;
        if(string.IsNullOrEmpty(collectionName))
            throw new ArgumentNullException(nameof(collectionName), "Необходимо указать название коллекции");
        await _settingsProvider.RemoveSettingsAsync(collectionName, name, cancellationToken);

        await _redisProvider.RemoveAsync(string.Format(REDIS_CACHE_KEY, name));
    }
}