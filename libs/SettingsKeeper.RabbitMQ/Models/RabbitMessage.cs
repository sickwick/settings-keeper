using System.Text.Json;

namespace SettingsKeeper.RabbitMQ.Models;


public class RabbitMessage
{
    // settings | featureToggle
    public string Version { get; set; }

    public JsonDocument Content { get; set; }
}