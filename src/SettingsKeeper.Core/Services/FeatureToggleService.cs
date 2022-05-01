using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.Provider.Abstract;
using SettingsKeeper.Provider.Models;

namespace SettingsKeeper.Core.Services;

public class FeatureToggleService : IFeatureToggleService
{
    private readonly IFeatureToogleProvider _featureToogleProvider;
    private readonly IRedisProvider _redisProvider;
    private readonly FeatureToogleSettings _options;

    public FeatureToggleService(IFeatureToogleProvider featureToogleProvider, IRedisProvider redisProvider,
        IOptions<SettingsKeeperSettings> options)
    {
        _featureToogleProvider = featureToogleProvider;
        _redisProvider = redisProvider;
        _options = options?.Value?.FeatureToogleSettings
                   ?? throw new ArgumentNullException(nameof(options));
    }

    // public async Task<bool> IsFeatureToogleEnabled(string name, CancellationToken cancellationToken)
    // {
    //     var result = await GetFeatureToogleFromCacheAsync(name, cancellationToken)
    //                  ?? await GetFeatureToogleFromMongoAsync(name, cancellationToken);
    //
    //     return result.Enabled;
    // }

    public async Task<FeatureToogle?> GetFeatureToggleAsync(string name, CancellationToken cancellationToken)
    {
        return await GetFeatureToogleFromCacheAsync(name, cancellationToken)
               ?? await GetFeatureToogleFromMongoAsync(name, cancellationToken);
    }

    public async Task AddFeatureToggleAsync(string name, string serviceName, JsonElement featureToggle,
        CancellationToken cancellationToken)
    {
        var content = new FeatureToogle()
        {
            Name = name, Content = JsonSerializer.Serialize(featureToggle), ServiceName = serviceName
        };
        await _featureToogleProvider.AddFeatureToggle(content, cancellationToken);
    }

    public async Task<FeatureToogle?> GetFeatureToogleFromMongoAsync(string name, CancellationToken cancellationToken)
    {
        return await _featureToogleProvider.GetFeatureToogle(name, cancellationToken);
    }

    public async Task<FeatureToogle?> GetFeatureToogleFromCacheAsync(string name, CancellationToken cancellationToken)
    {
        return await _redisProvider.GetAsync<FeatureToogle>(name);
    }

    public async Task<FeatureToggles> GetAllFeatureToggles(string serviceName, CancellationToken cancellationToken)
    {
        var ft = await _featureToogleProvider.GetAllFeatureToogles(serviceName, cancellationToken);
        var data = ft.Select(c => c.Content);
        var sb = new StringBuilder();
        foreach (var item in data)
            sb.Append(item).Append(',');

        return new FeatureToggles() { FeatureManagement = JsonDocument.Parse(sb.Remove(sb.Length - 1, 1).ToString()) };
    }

    public async Task EditFeatureToggleSettingsAsync(string name, JsonElement featureToggle,
        CancellationToken cancellationToken)
    {
        var data = await GetFeatureToggleAsync(name, cancellationToken);
        data.Content = JsonSerializer.Serialize(featureToggle);
        await _featureToogleProvider.EditFeatureToggle(data, cancellationToken);
    }

    public async Task RemoveFeatureToggleAsync(string name, CancellationToken cancellationToken)
    {
        await _featureToogleProvider.RemoveFeatureToggleAsync(name, cancellationToken);
    }

    public class FeatureToggles
    {
        [JsonPropertyName("FeatureManagement")]
        public JsonDocument FeatureManagement { get; set; }
    }
}