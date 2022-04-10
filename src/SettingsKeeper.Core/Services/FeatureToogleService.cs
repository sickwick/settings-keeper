using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.Provider.Abstract;
using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Core.Services;

public class FeatureToogleService: IFeatureToogleService
{
    private readonly IFeatureToogleProvider _featureToogleProvider;
    private readonly IRedisProvider _redisProvider;

    public FeatureToogleService(IFeatureToogleProvider featureToogleProvider, IRedisProvider redisProvider)
    {
        _featureToogleProvider = featureToogleProvider;
        _redisProvider = redisProvider;
    }

    public async Task<FeatureToogle> GetFeatureToogleFromMongoAsync(string name, CancellationToken cancellationToken)
    {
        return await _featureToogleProvider.GetFeatureToogle(name, cancellationToken);
    }

    public async Task<FeatureToogle> GetFeatureToogleFromCacheAsync(string name, CancellationToken cancellationToken)
    {
        return await _redisProvider.GetAsync<FeatureToogle>(name);
    }
}