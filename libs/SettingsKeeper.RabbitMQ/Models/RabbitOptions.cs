using System.ComponentModel.DataAnnotations;

namespace SettingsKeeper.RabbitMQ.Models;

public class RabbitOptions
{
    [Required] public string HostName { get; set; }
}