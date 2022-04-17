using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using SettingsKeeper.Client.Providers;

namespace SettingsKeeper.Client.Extensions;

public static class AppsettingsConfigurationExtension
{
    public static IConfigurationBuilder AddSettingsFile(this IConfigurationBuilder builder, Action<AppsettingsConfigurationSource> configureSource)
        => builder.Add(configureSource);
    
    public static IConfigurationBuilder AddSettingsFile(this IConfigurationBuilder builder, IFileProvider provider, string path, bool optional, bool reloadOnChange)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        return builder.AddSettingsFile(s =>
        {
            s.FileProvider = provider;
            s.Path = path;
            s.Optional = optional;
            s.ReloadOnChange = reloadOnChange;
            s.ResolveFileProvider();
        });
    }
    
    public static IConfigurationBuilder AddSettingsFile(this IConfigurationBuilder builder, string path, bool optional = false, bool reloadOnChange = true)
    {
        return AddSettingsFile(builder, provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);
    }
}