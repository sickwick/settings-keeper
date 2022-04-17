using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.RabbitMQ.Abstract;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController:ControllerBase
{
    private readonly ISettingsService _settingsService;
    public SettingsController(
        ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<IActionResult> GetSetting(string name, CancellationToken cancellationToken)
    {
        var response = await _settingsService.GetSettingsAsync(name, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddSetting([FromQuery] string name, [FromBody] JsonElement settings, CancellationToken cancellationToken)
    {
        var set = JsonSerializer.Serialize(settings);
        await _settingsService.AddSettingsAsync(name, set, cancellationToken);
        return NoContent();
    }
    
    [HttpPut]
    [Route("{name}")]
    public IActionResult EditSetting([FromBody] string settings)
    {
        return Ok();
    }
    
    [HttpDelete]
    [Route("{name}")]
    public async Task<IActionResult> DeleteSetting(string name)
    {
        return Ok();
    }
}