using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.RabbitMQ.Abstract;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("")]
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
    public IActionResult AddSetting([FromBody] string settings)
    {
        return Ok();
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