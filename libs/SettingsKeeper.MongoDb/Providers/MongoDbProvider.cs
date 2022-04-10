using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.MongoDb.Models;

namespace SettingsKeeper.MongoDb.Providers;

internal sealed class MongoDbProvider : IMongoDbProvider
{
    public MongoClient GetMongoClient([FromServices] IOptions<MongoSettings> mongoSettings)
    {
        // var settings = mongoSettings?.Value ?? throw new ArgumentNullException(nameof(mongoSettings));
        var settings = "mongodb://localhost:27017";

        // var connectionString = settings.ConnectionString 
        // ?? throw new ArgumentNullException(nameof(settings.ConnectionString));

        // var client = new MongoClient(connectionString);
        var client = new MongoClient(settings);

        return client;
    }
}