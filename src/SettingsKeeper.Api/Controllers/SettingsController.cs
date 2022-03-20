using Microsoft.AspNetCore.Mvc;
using SettingsKeeper.Api.Models;
using SettingsKeeper.Core.Abstract;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("")]
public class SettingsController:ControllerBase
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }
    
    [HttpPost]
    public IActionResult CreateFeatureToogle([FromBody] FeatureToogle featureToogle)
    {
        return NoContent();
    }

    [HttpGet]
    [Route("{name}")]
    public IActionResult GetFeatureToogle(string name)
    {
        return Ok();
    }
    
    [HttpPut]
    [Route("{name}")]
    public IActionResult EditFeatureToogle(string name)
    {
        return Ok();
    }
    
    [HttpDelete]
    [Route("{name}")]
    public IActionResult DeleteFeatureToogle(string name)
    {
        return Ok();
    }
}