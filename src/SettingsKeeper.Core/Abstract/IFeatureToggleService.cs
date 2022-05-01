using System.Text.Json;
using SettingsKeeper.Core.Services;
using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Core.Abstract;

public interface IFeatureToggleService
{
    Task<FeatureToogle?> GetFeatureToggleAsync(string name, CancellationToken cancellationToken);
    Task<FeatureToggleService.FeatureToggles> GetAllFeatureToggles(string serviceName, CancellationToken cancellationToken);

    Task AddFeatureToggleAsync(string name, string serviceName, JsonElement featureToggle,
        CancellationToken cancellationToken);

    Task EditFeatureToggleSettingsAsync(string name, JsonElement featureToggle,
        CancellationToken cancellationToken);

    Task RemoveFeatureToggleAsync(string name, CancellationToken cancellationToken);
}