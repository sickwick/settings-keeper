using SettingsKeeper.MongoDb.Abstract;

namespace SettingsKeeper.Provider.Models;

public class FeatureToogle: ISimpleCollection
{
    public string Id { get; set; }
    public string Name { get; set; }

    public bool Enabled { get; set; }

    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
}