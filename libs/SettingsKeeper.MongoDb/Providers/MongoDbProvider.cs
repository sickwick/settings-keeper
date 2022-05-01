using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.MongoDb.Exceptions;
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
        where T : ISimpleCollection
    {
        try
        {
            _ = await GetElementByNameAsync<T>(data.Name, collectionName, cancellationToken);
            throw new NameContainsException();
        }
        catch (NameContainsException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogDebug("Подавленная ошибка при добавлении записи");
        }
        
        var collection = GetMongoCollection<T>(collectionName);
        await collection.InsertOneAsync(data, new InsertOneOptions(), cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllElementsByServiceNameAsync<T>(string collectionName, string name, CancellationToken cancellationToken)
        where T : ISimpleCollection
    {
        var collection = GetMongoCollection<T>(collectionName);
        var cursor = await collection.FindAsync(Builders<T>.Filter.Eq(c=>c.ServiceName, name), cancellationToken: cancellationToken);
        return await cursor.ToListAsync(cancellationToken);
    }

    public async Task SetElementAsync<T>(string collectionName, string name, T data, CancellationToken cancellationToken)
        where T : ISimpleCollection
    {
        var collection = GetMongoCollection<T>(collectionName);
        var getId = await GetElementByFilterAsync<T>(collectionName, Builders<T>.Filter.Eq(c=>c.Name, name), cancellationToken);
        data.Id = getId.Id;
        var result = await collection.ReplaceOneAsync(
            Builders<T>.Filter.Eq(c => c.Id, getId.Id), 
            data,
            cancellationToken: cancellationToken);
        
        _logger.LogInformation(string.Format("найдено по соответствию: {0}; обновлено: {1}", result.MatchedCount, result.ModifiedCount));
    }

    public async Task RemoveElementAsync<T>(string collectionName, string name, CancellationToken cancellationToken)
        where T : ISimpleCollection
    {
        var collection = GetMongoCollection<T>(collectionName);
        await collection.DeleteOneAsync(Builders<T>.Filter.Eq(c=>c.Name, name), cancellationToken);
        
        _logger.LogInformation($"Удален элемент.(name: {name})");
    }
}