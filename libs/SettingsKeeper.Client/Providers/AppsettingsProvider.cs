using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using SettingsKeeper.Client.Models;
using SettingsKeeper.Client.Utils;

namespace SettingsKeeper.Client.Providers;

public class AppsettingsProvider: JsonConfigurationProvider
{
    public override void Load()
    {
        var path = Directory.GetCurrentDirectory();
        var config = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json")
            .Build()
            .GetSection("SettingsKeeperClient")
            .Get<ClientOptions>();
        
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(config.BaseUrl);
        var request = UrlUtils.BuildPath(config.SettingsPath, config.AppName);
        var response = httpClient.GetAsync(request).GetAwaiter().GetResult();

        if (!response.IsSuccessStatusCode)
            throw new Exception("Запрос выполнился с ошибкой");

        var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        var bytes = Encoding.UTF8.GetBytes(result);
        using var stream = new MemoryStream(bytes);
        base.Load(stream);
    }

    public AppsettingsProvider(JsonConfigurationSource source) : base(source)
    {
    }
}