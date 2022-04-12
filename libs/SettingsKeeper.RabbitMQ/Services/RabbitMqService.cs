using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SettingsKeeper.RabbitMQ.Abstract;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.RabbitMQ.Services;

public class RabbitMqService : IRabbitMqService
{
    private readonly ILogger _logger;
    private readonly RabbitOptions _options;

    public RabbitMqService(ILogger<RabbitMqService> logger,
        IOptions<RabbitOptions> options)
    {
        _logger = logger;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public void CreateNewRabbitQueue(string queueName)
    {
        var factory = new ConnectionFactory() { HostName = _options.HostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _logger.LogInformation($"Create queue for {queueName}");
    }

    public void SendMessage<T>(string queueName, T message)
        where T : class
    {
        var factory = new ConnectionFactory() { HostName = _options.HostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        var data = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(data);
        
        channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        channel.BasicPublish(exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: null,
            body: body);

        _logger.LogInformation($"Send message to {queueName}");
    }
}