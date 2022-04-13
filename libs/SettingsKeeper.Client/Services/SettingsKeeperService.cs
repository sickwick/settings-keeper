using System.Text;
using Microsoft.Extensions.Logging;
using SettingsKeeper.RabbitMQ.Abstract;

namespace SettingsKeeper.Client.Services;

public class SettingsKeeperService : IRabbitMqResult
{
    private readonly ILogger<SettingsKeeperService> _logger;

    public SettingsKeeperService(ILogger<SettingsKeeperService> logger)
    {
        _logger = logger;
    }
    public void UseRabbitMessageResult(string message)
    {
        Console.WriteLine(message);
    }
}