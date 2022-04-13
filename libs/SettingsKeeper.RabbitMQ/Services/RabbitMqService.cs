using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SettingsKeeper.RabbitMQ.Abstract;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.RabbitMQ.Services;

public class RabbitMqService : IRabbitMqService, IDisposable
{
    private readonly ILogger _logger;
    private readonly RabbitOptions _options;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqService(ILogger<RabbitMqService> logger,
        IOptions<RabbitOptions> options)
    {
        _logger = logger;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        var factory = new ConnectionFactory { HostName = _options.HostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _declaredQueue = new HashSet<string>();
    }

    private HashSet<string> _declaredQueue;

    public void CreateNewRabbitQueue(string queueName)
    {
        if (_declaredQueue.Contains(queueName))
            throw new Exception("Очередь уже существует");
        _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _declaredQueue.Add(queueName);

        _logger.LogInformation($"Create queue for {queueName}");
    }

    public void SendMessage<T>(string queueName, T message)
        where T : class
    {
        var data = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(data);

        if(!_declaredQueue.Contains(queueName))
            CreateNewRabbitQueue(queueName);

        _channel.BasicPublish(exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: null,
            body: body);

        _logger.LogInformation($"Send message to {queueName}");
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}