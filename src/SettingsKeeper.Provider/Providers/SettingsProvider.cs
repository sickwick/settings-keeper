using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.Provider.Abstract;
using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Provider.Providers;

public class SettingsProvider: ISettingsProvider
{
    private readonly IMongoDbProvider _mongoDbProvider;

    public SettingsProvider(IMongoDbProvider mongoDbProvider)
    {
        _mongoDbProvider = mongoDbProvider;
    }

    public async Task<Settings> GetSettingsAsync(string name, string collectionName, CancellationToken cancellationToken)
    {
        return await _mongoDbProvider.GetElementByNameAsync<Settings>(name, collectionName, cancellationToken);
    }
}