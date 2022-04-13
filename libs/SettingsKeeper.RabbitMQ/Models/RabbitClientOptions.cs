using System.ComponentModel.DataAnnotations;

namespace SettingsKeeper.RabbitMQ.Models;

public class RabbitClientOptions
{
    [Required] public string HostName { get; set; }
    [Required] public string QueueName { get; set; }
}