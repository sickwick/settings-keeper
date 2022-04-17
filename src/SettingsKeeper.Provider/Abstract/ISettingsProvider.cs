using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Provider.Abstract;

public interface ISettingsProvider
{
    Task<Settings> GetSettingsAsync(string name, string collectionName, CancellationToken cancellationToken);
}