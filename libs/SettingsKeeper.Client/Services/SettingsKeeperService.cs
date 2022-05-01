using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using SettingsKeeper.RabbitMQ.Abstract;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.Client.Services;

public class SettingsKeeperService : IRabbitMqResult
{
    private readonly ILogger<SettingsKeeperService> _logger;

    public SettingsKeeperService(ILogger<SettingsKeeperService> logger)
    {
        _logger = logger;
    }
    public void UseRabbitMessageResult(RabbitMessage message)
    {
        string workingDirectory = Environment.CurrentDirectory;
        var content = JsonSerializer.Serialize(message.Content);
        switch (message.Version)
        {
            case "settings":
                File.WriteAllText(Path.Combine(workingDirectory,"settings.json"), content);
                break;
            case "featureToggle":
                File.WriteAllText(Path.Combine(workingDirectory,"featureToggles.json"), content);
                break;
        }
    }
}