using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.MongoDb.Models;

namespace SettingsKeeper.MongoDb.Providers;

internal sealed class MongoDbProvider : IMongoDbProvider
{
    private readonly ILogger<MongoDbProvider> _logger;
    private readonly MongoSettings _mongoSettings;

    public MongoDbProvider(IOptions<MongoSettings> mongoSettings, ILogger<MongoDbProvider> logger)
    {
        _logger = logger;
        _mongoSettings = mongoSettings?.Value ?? throw new ArgumentNullException(nameof(mongoSettings));
    }

    private IMongoDatabase GetDatabase()
    {
        var client = new MongoClient(_mongoSettings.ConnectionString);
        var db = client.GetDatabase(_mongoSettings.DataBaseName);
        return db;
    }

    private IMongoCollection<T> GetMongoCollection<T>(string collectionName)
    {
        var db = GetDatabase();
        var collection = db.GetCollection<T>(collectionName);

        return collection;
    }

    public async Task<IEnumerable<T>> GetCollectionAsync<T>(string collectionName, CancellationToken cancellationToken)
    {
        var collection = GetMongoCollection<T>(collectionName);
        var cursor = await collection.FindAsync(Builders<T>.Filter.Empty, cancellationToken: cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }

    public async Task<T> GetElementByNameAsync<T>(string name, string collectionName,
        CancellationToken cancellationToken)
        where T : ISimpleCollection
    {
        return await GetElementByFilterAsync(collectionName, Builders<T>.Filter.Eq(c => c.Name, name),
            cancellationToken);
    }

    public async Task<T> GetElementByIdAsync<T>(string id, string collectionName, CancellationToken cancellationToken)
        where T : ISimpleCollection
    {
        return await GetElementByFilterAsync(collectionName, Builders<T>.Filter.Eq(c => c.Id, id), cancellationToken);
    }

    public async Task<T> GetElementByFilterAsync<T>(string collectionName, FilterDefinition<T> filter,
        CancellationToken cancellationToken)
        where T : ISimpleCollection
    {
        var collection = GetMongoCollection<T>(collectionName);
        var cursor = await collection.FindAsync(filter, cancellationToken: cancellationToken);
        var result = await cursor.FirstOrDefaultAsync(cancellationToken);

        _logger.LogInformation($"Получено значение переключателя: {result.Name}", result);

        return result;
    }

    public async Task AddElementAsync<T>(string collectionName, T data, CancellationToken cancellationToken)
    {
        var content = JsonSerializer.Serialize(data);
        var collection = GetMongoCollection<T>(collectionName);
        await collection.InsertOneAsync(data, new InsertOneOptions(), cancellationToken);
    }
}