using System.ComponentModel.DataAnnotations;

namespace SettingsKeeper.Provider.Models;

public class FeatureToogleSettings
{
    [Required] public string CollectionName { get; set; }
}