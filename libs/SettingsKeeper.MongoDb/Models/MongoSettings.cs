using System.ComponentModel.DataAnnotations;

namespace SettingsKeeper.MongoDb.Models;

public class MongoSettings
{
    [Required] public string ConnectionString { get; set; }

    [Required] public string DataBaseName { get; set; }
}