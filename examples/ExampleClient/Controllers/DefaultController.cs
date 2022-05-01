using ExampleClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
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
    public IActionResult Test([FromServices] IConfiguration configuration)
    {
        return Ok(configuration.GetValue<string>("TestSettings:Color"));
    }
}