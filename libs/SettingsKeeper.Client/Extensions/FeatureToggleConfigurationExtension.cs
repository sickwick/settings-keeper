using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using SettingsKeeper.Client.Providers;

namespace SettingsKeeper.Client.Extensions;

public static class FeatureToggleConfigurationExtension
{
    public static IConfigurationBuilder AddFeatureToggleFile(this IConfigurationBuilder builder, Action<FeatureToggleConfigurationSource> configureSource)
        => builder.Add(configureSource);
    
    public static IConfigurationBuilder AddFeatureToggleFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        return builder.AddFeatureToggleFile(s =>
        {
            s.FileProvider = provider;
            s.Path = path;
            s.Optional = optional;
            s.ReloadOnChange = reloadOnChange;
            s.ResolveFileProvider();
        });
    }
    
    public static IConfigurationBuilder AddFeatureToggleFile(this IConfigurationBuilder builder, string path, bool optional = false, bool reloadOnChange = true)
    {
        return AddFeatureToggleFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
    }
}