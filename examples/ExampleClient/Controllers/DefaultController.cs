using ExampleClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Configuration.Memory;
using SettingsKeeper.Client.Providers;
using SO=System.IO.File;

namespace ExampleClient.Controllers;

[ApiController]
[Route("")]
public class DefaultController: ControllerBase
{
    private readonly IFeatureManager _featureManager;

    public DefaultController(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }
    [HttpGet]
    public async Task<IActionResult> SayHello()
    {
        if (await _featureManager.IsEnabledAsync(FeatureToogles.FeatureA))
        {
            return Ok("Hello");
        }

        return NoContent();
    }

    [HttpGet]
    [Route("test")]
    public IActionResult Test(bool isEnabled, [FromServices] IConfiguration configuration, [FromServices]IServiceProvider provider)
    {
        var a = new AppsettingsProvider();
        return Ok(a.test(isEnabled, configuration, provider));
    }
}