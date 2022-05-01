using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SettingsKeeper.RabbitMQ.Abstract;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.RabbitMQ.Services;

public class RabbitMqListener: BackgroundService
{
    private IConnection _connection;
    private IModel _channel;
    private readonly ILogger _logger;
    private readonly IRabbitMqResult _result;
    private readonly RabbitClientOptions _options;
    
    public RabbitMqListener(IOptions<RabbitClientOptions> options, ILogger<RabbitMqListener> logger, IRabbitMqResult result)
    {
        _logger = logger;
        _result = result;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        
        var factory = new ConnectionFactory { HostName = _options.HostName };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _options.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var res = JsonDocument.Parse(ea.Body.ToArray());
            var data = JsonSerializer.Deserialize<RabbitMessage>(res.RootElement.GetString());

            _result.UseRabbitMessageResult(data);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(_options.QueueName, false, consumer);

        return Task.CompletedTask;
    }
	
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}