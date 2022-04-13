using System.ComponentModel.DataAnnotations;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.Client.Models;

public class ClientOptions
{
    [Required] public string BaseUrl { get; set; }
    [Required] public string ServerPath { get; set; }
}