using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Provider.Abstract;

public interface IFeatureToogleProvider
{
    Task<FeatureToogle> GetFeatureToogle(string name, CancellationToken cancellationToken);

    Task<IEnumerable<FeatureToogle>> GetAllFeatureToogles(string serviceName, CancellationToken cancellationToken);
    
    Task AddFeatureToggle(FeatureToogle content, CancellationToken cancellationToken);

    Task EditFeatureToggle(FeatureToogle content, CancellationToken cancellationToken);

    Task RemoveFeatureToggleAsync(string serviceName, CancellationToken cancellationToken);
}