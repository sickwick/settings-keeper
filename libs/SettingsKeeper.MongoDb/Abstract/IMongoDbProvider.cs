using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SettingsKeeper.MongoDb.Models;

namespace SettingsKeeper.MongoDb.Abstract;

public interface IMongoDbProvider
{
    MongoClient GetMongoClient([FromServices] IOptions<MongoSettings> mongoSettings);
}