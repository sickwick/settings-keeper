using System.ComponentModel.DataAnnotations;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.Client.Models;

public class ClientOptions
{
    [Required] public string BaseUrl { get; set; }
    [Required] public string ServerPath { get; set; }

    [Required] public string FeatureTooglePath { get; set; }
    
    [Required] public string SettingsPath { get; set; }
    
    [Required] public string AppName { get; set; }
}