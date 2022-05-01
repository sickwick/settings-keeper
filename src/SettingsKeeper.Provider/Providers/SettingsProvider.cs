using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.Provider.Abstract;
using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Provider.Providers;

public class SettingsProvider : ISettingsProvider
{
    private readonly IMongoDbProvider _mongoDbProvider;

    public SettingsProvider(IMongoDbProvider mongoDbProvider)
    {
        _mongoDbProvider = mongoDbProvider;
    }

    public async Task<Settings> GetSettingsAsync(string name, string collectionName,
        CancellationToken cancellationToken)
    {
        return await _mongoDbProvider.GetElementByNameAsync<Settings>(name, collectionName, cancellationToken);
    }

    public async Task AddSettingsAsync(string collectionName, Settings settings, CancellationToken cancellationToken)
    {
        await _mongoDbProvider.AddElementAsync(collectionName, settings, cancellationToken);
    }

    public async Task ChangeSettingsAsync(string collectionName, string name, Settings settings,
        CancellationToken cancellationToken)
    {
        await _mongoDbProvider.SetElementAsync(collectionName, name, settings, cancellationToken);
    }

    public async Task RemoveSettingsAsync(string collectionName, string name, CancellationToken cancellationToken)
    {
        await _mongoDbProvider.RemoveElementAsync<Settings>(collectionName, name, cancellationToken);
    }
}