using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SettingsKeeper.Client.Constants;
using SettingsKeeper.Client.Models;
using SettingsKeeper.Client.Utils;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.Client.Services;

public class AppRegistratorHostedService: BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RabbitClientOptions _rabbitOptions;
    private readonly ClientOptions _options;

    public AppRegistratorHostedService(
        IHttpClientFactory httpClientFactory, 
        IOptions<ClientOptions> options, 
        IOptions<RabbitClientOptions> rabbitOptions)
    {
        _httpClientFactory = httpClientFactory;
        _rabbitOptions = rabbitOptions?.Value ?? throw new ArgumentNullException(nameof(rabbitOptions));;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var httpClient = _httpClientFactory.CreateClient(ClientConstants.HttpClient);
        var path = UrlUtils.BuildPath(_options.ServerPath, _rabbitOptions.QueueName);
        using var response = await httpClient.GetAsync(path, stoppingToken);
        
        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
    }
}