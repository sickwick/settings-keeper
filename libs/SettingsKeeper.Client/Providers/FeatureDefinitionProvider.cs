using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using SettingsKeeper.Client.Abstraction;

namespace SettingsKeeper.Client.Providers;

public class FeatureDefinitionProvider : IFeatureDefinitionProvider
{
    private readonly IFeatureToogleService _featureToogleService;

    public FeatureDefinitionProvider(IConfiguration configuration, IFeatureToogleService featureToogleService)
    {
        _featureToogleService = featureToogleService;
    }

    public async Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
    {
        var result =  await _featureToogleService.IsFeatureToogleEnabled(featureName, CancellationToken.None);
        if (result)
        {
            return new FeatureDefinition()
            {
                Name = featureName,
                EnabledFor = new List<FeatureFilterConfiguration>()
                {
                    new FeatureFilterConfiguration()
                    {
                        Name = "AlwaysOn"
                    }
                }
            };
        }
        return new FeatureDefinition()
        {
            Name = featureName
        };
    }

    public IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
    {
        throw new NotImplementedException();
    }
}