using Microsoft.AspNetCore.Mvc;
using SettingsKeeper.Api.Models;
using SettingsKeeper.Core.Abstract;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("featureToogle")]
public class FeatureToogleController:ControllerBase
{
    private readonly IFeatureToogleService _featureToogleService;

    public FeatureToogleController(IFeatureToogleService featureToogleService)
    {
        _featureToogleService = featureToogleService;
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
        return Ok(false);
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