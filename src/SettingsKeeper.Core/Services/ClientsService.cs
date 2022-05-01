using System.Text.Json;
using Microsoft.Extensions.Logging;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.RabbitMQ.Abstract;

namespace SettingsKeeper.Core.Services;

public class ClientsService: IClientsService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly IRedisProvider _redisProvider;
    private readonly ILogger _logger;
    private const string REDIS_CLIENTS_KEY = "clients";

    public ClientsService(IRabbitMqService rabbitMqService, IRedisProvider redisProvider, ILogger<ClientsService> logger)
    {
        _rabbitMqService = rabbitMqService;
        _redisProvider = redisProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<string>> GetAllClients()
    {
        var clients = await _redisProvider.GetAsync<List<string>>(REDIS_CLIENTS_KEY);
        if (clients is null || clients?.Count == 0)
            clients = new List<string>();
        return clients.ToList();
    }

    public async Task AddNewClient(string name)
    {
        var clients = await _redisProvider.GetAsync<List<string>>(REDIS_CLIENTS_KEY);
        if (clients is null || clients?.Count == 0)
            clients = new List<string>();

        if (clients?.Any(c => c.Equals(name)) == true)
            throw new Exception("Пользователь с таким именем уже существует");
        
        clients.Add(name);
        await _redisProvider.SetAsync(REDIS_CLIENTS_KEY, clients);
        _rabbitMqService.CreateNewRabbitQueue(name);
    }

    public void SendMessageToClient<T>(string name, T message)
    where T: class
    {
        if (message is JsonDocument json)
        {
            var data = JsonSerializer.Serialize(json);
            _rabbitMqService.SendMessage(name, data);
        }
        _rabbitMqService.SendMessage(name, message);
    }
    
    public async Task SendMessageToAllClients<T>(T message)
        where T: class
    {
        var clients = await _redisProvider.GetAsync<List<string>>(REDIS_CLIENTS_KEY);
        if (clients?.Count == 0)
            throw new Exception("В список не добавлено ни одного пользователя");
        
        foreach(var client in clients)
            SendMessageToClient(client, message);

    }
}