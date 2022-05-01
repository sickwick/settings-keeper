using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SettingsKeeper.Core.Abstract;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("featureToggle")]
public class FeatureToggleController : ControllerBase
{
    private readonly IFeatureToggleService _featureToggleService;

    public FeatureToggleController(IFeatureToggleService featureToggleService)
    {
        _featureToggleService = featureToggleService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFeatureToggle([FromQuery] string name, [FromQuery] string serviceName,
        [FromBody] JsonElement featureToggle, CancellationToken cancellationToken)
    {
        await _featureToggleService.AddFeatureToggleAsync(name, serviceName, featureToggle, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAllFeatureToggles(string name, CancellationToken cancellationToken)
    {
        var result = await _featureToggleService.GetAllFeatureToggles(name, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<IActionResult> GetFeatureToggle(string name, CancellationToken cancellationToken)
    {
        return Ok(await _featureToggleService.GetFeatureToggleAsync(name, cancellationToken));
    }

    [HttpPut]
    public async Task<IActionResult> EditFeatureToggle([FromQuery] string name, [FromBody] JsonElement featureToggle,
        CancellationToken cancellationToken)
    {
        await _featureToggleService.EditFeatureToggleSettingsAsync(name, featureToggle, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFeatureToggle([FromQuery] string name, CancellationToken cancellationToken)
    {
        await _featureToggleService.RemoveFeatureToggleAsync(name, cancellationToken);
        return NoContent();
    }
}