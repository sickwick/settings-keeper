using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SettingsKeeper.Client.Abstraction;
using SettingsKeeper.Client.Constants;
using SettingsKeeper.Client.Models;
using SettingsKeeper.Client.Utils;

namespace SettingsKeeper.Client.Services;

public class FeatureToogleService : IFeatureToogleService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FeatureToogleService> _logger;
    private readonly ClientOptions _options;

    public FeatureToogleService(IHttpClientFactory httpClientFactory, IOptions<ClientOptions> options,
        ILogger<FeatureToogleService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<bool> IsFeatureToogleEnabled(string name, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(ClientConstants.HttpClient);
        var path = UrlUtils.BuildPath(_options.FeatureTooglePath, name);
        using var response = await httpClient.GetAsync(path, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());

        bool isEnabled = false;
        try
        {
            isEnabled = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
        }
        catch (NotSupportedException ex)
        {
            _logger.LogError(exception: ex, message: "Не удалось получить значение переключателя",
                await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken));
        }

        return isEnabled;
    }
}