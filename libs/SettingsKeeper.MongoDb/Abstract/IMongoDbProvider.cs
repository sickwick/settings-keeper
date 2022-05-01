using MongoDB.Driver;

namespace SettingsKeeper.MongoDb.Abstract;

public interface IMongoDbProvider
{
    Task<IEnumerable<T>> GetCollectionAsync<T>(string collectionName, CancellationToken cancellationToken);

    Task<T> GetElementByNameAsync<T>(string name, string collectionName, CancellationToken cancellationToken)
        where T : ISimpleCollection;

    Task<T> GetElementByIdAsync<T>(string id, string collectionName, CancellationToken cancellationToken)
        where T : ISimpleCollection;

    Task<T> GetElementByFilterAsync<T>(string collectionName, FilterDefinition<T> filter,
        CancellationToken cancellationToken) where T : ISimpleCollection;

    Task<IEnumerable<T>> GetAllElementsByServiceNameAsync<T>(string collectionName, string name,
        CancellationToken cancellationToken)
        where T : ISimpleCollection;

    Task AddElementAsync<T>(string collectionName, T data, CancellationToken cancellationToken)
        where T : ISimpleCollection;

    Task SetElementAsync<T>(string collectionName, string name, T data, CancellationToken cancellationToken)
        where T : ISimpleCollection;

    Task RemoveElementAsync<T>(string collectionName, string name, CancellationToken cancellationToken)
        where T : ISimpleCollection;
}