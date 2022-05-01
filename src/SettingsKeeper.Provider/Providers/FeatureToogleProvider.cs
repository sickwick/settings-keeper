using Microsoft.Extensions.Options;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.Provider.Abstract;
using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Provider.Providers;

public class FeatureToogleProvider: IFeatureToogleProvider
{
    private readonly IMongoDbProvider _mongoProvider;
    private readonly FeatureToogleSettings _featureToogleSettings;

    public FeatureToogleProvider(IMongoDbProvider mongoProvider, IOptions<SettingsKeeperSettings> options)
    {
        _mongoProvider = mongoProvider;
        _featureToogleSettings = options?.Value?.FeatureToogleSettings
                                 ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<FeatureToogle> GetFeatureToogle(string name, CancellationToken cancellationToken)
    {
        var featueToogle = await _mongoProvider.GetElementByNameAsync<FeatureToogle>(name, _featureToogleSettings.CollectionName,
            cancellationToken);
        return featueToogle;
    }

    public async Task<IEnumerable<FeatureToogle>> GetAllFeatureToogles(string serviceName, CancellationToken cancellationToken)
    {
        var featureToogles = await _mongoProvider.GetAllElementsByServiceNameAsync<FeatureToogle>(_featureToogleSettings.CollectionName, serviceName, cancellationToken);
        return featureToogles;
    }

    public async Task AddFeatureToggle(FeatureToogle content, CancellationToken cancellationToken)
    {
        await _mongoProvider.AddElementAsync(_featureToogleSettings.CollectionName, content,
            cancellationToken);
    }

    public async Task EditFeatureToggle(FeatureToogle content, CancellationToken cancellationToken)
    {
        await _mongoProvider.SetElementAsync(_featureToogleSettings.CollectionName, content.Name, content,
            cancellationToken);
    }

    public async Task RemoveFeatureToggleAsync(string serviceName, CancellationToken cancellationToken)
    {
        await _mongoProvider.RemoveElementAsync<FeatureToogle>(_featureToogleSettings.CollectionName, serviceName, cancellationToken);
    }
}