using SettingsKeeper.MongoDb.Abstract;

namespace SettingsKeeper.Provider.Models;

public class Settings: ISimpleCollection
{
    public string Id { get; set; }
    public string Name { get; set; }

    public string AppSettings { get; set; }
}