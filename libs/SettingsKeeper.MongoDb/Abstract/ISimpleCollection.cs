namespace SettingsKeeper.MongoDb.Abstract;

public interface ISimpleCollection
{
    string Id { get; set; }
    string Name { get; set; }
}