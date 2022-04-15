using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using SettingsKeeper.RabbitMQ.Models;

namespace SettingsKeeper.Client.Providers;

public class AppsettingsProvider
{
    public string test(bool isEnabled, IConfiguration configuration, IServiceProvider col)
    {
        // var a = configuration.GetSection("FeatureManagement");
        // var path = Directory.GetParent(Environment.CurrentDirectory);
        var path = Directory.GetCurrentDirectory();
        var config = new ConfigurationBuilder()
            // .SetBasePath(path)
            // .AddJsonFile("featureToogle.json", optional: true, reloadOnChange: true)
            .Add(new MemoryConfigurationSource(){InitialData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("FeatureManagement:FeatureA", "false")
            }})
            .Build();
        var d = new ConfigurationReloadToken();
        d.OnReload();
        // config["FeatureManagement:FeatureA"] = "false";
        //
        // var en = config.GetSection("FeatureManagement");
        // var d = en.AsEnumerable().ToList();
        // var m = JsonConvert.SerializeObject(d);
        // var opt = new JsonSerializerOptions()
        // {
        //     WriteIndented = true
        // };

        // var json = JsonConvert.SerializeObject(config);
        // var appPath = Path.Combine(path, "settings.json");
        // File.WriteAllText(appPath, json);
        // var configProv = new SettingsKeeper();
        // if(configProv.TryGet("FeatureManagement:FeatureA", out string result))
        //     configProv.Set("FeatureManagement:FeatureA", "false");
        // configuration.Bind("FeatureManagement:FeatureA", "false");
        // configuration["FeatureManagement:FeatureA"] = isEnabled.ToString();
        // config.Reload();
        // var kv = configuration.AsEnumerable().ToList();
        // var serv = col.GetServices<IFeatureDefinitionProvider>();
        // foreach (var service in serv)
        // {
        //     if (service is IDisposable)
        //         (service as IDisposable).Dispose();
        // }
        //
        // var a = new MemoryConfigurationProvider(new MemoryConfigurationSource());
        // a.TryGet("FeatureManagement:FeatureA", out string value);
        // a.Set("FeatureManagement:FeatureA", "false");
        return "false";
    }
}

class SettingsKeeper : ConfigurationProvider
{
    public override void Load()
    {
        base.Load();
    }

    public override void Set(string key, string value)
    {
        base.Set(key, value);
    }

    public override bool TryGet(string key, out string value)
    {
        return base.TryGet(key, out value);
    }
}