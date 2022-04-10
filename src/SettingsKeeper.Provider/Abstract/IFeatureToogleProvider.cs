using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Provider.Abstract;

public interface IFeatureToogleProvider
{
    Task<FeatureToogle> GetFeatureToogle(string name, CancellationToken cancellationToken);
}