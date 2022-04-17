using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SettingsKeeper.MongoDb.Abstract;

namespace SettingsKeeper.Provider.Models;

public class Settings: ISimpleCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }

    public string AppSettings { get; set; }
}