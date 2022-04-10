using System.ComponentModel.DataAnnotations;

namespace SettingsKeeper.Provider.Models;

public class SettingsKeeperSettings
{
    [Required] public FeatureToogleSettings FeatureToogleSettings { get; set; }
}