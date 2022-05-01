using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using SettingsKeeper.Client.Models;
using SettingsKeeper.Client.Utils;

namespace SettingsKeeper.Client.Providers;

public class FeatureToggleProvider: JsonConfigurationProvider
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
        QueryBuilder b = new QueryBuilder();
        b.Add("name", config.AppName);
        var c = b.ToQueryString();
        var request = UrlUtils.BuildPath(config.FeatureTooglePath, c.ToString());
        var response = httpClient.GetAsync(request).GetAwaiter().GetResult();

        if (!response.IsSuccessStatusCode)
            throw new Exception("Запрос выполнился с ошибкой");

        var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        File.WriteAllText(Source.Path, result);
        var bytes = Encoding.UTF8.GetBytes(result);
        using var stream = new MemoryStream(bytes);
        base.Load(stream);
    }

    public FeatureToggleProvider(JsonConfigurationSource source) : base(source)
    {
    }
}