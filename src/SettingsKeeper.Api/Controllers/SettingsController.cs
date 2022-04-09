using Microsoft.AspNetCore.Mvc;
using SettingsKeeper.Api.Models;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("")]
public class SettingsController:ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly ISettingsKeeperCacheProvider _cacheProvider;

    public SettingsController(ISettingsService settingsService, ISettingsKeeperCacheProvider cacheProvider)
    {
        _settingsService = settingsService;
        _cacheProvider = cacheProvider;
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<IActionResult> GetSetting(string name)
    {
        await _cacheProvider.SetAsync(name, $"{name}-{name}");
        var res = await _cacheProvider.GetAsync(name);
        return Ok(res);
    }
    
    [HttpPost]
    [Route("{name}")]
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
        var res = await _cacheProvider.RemoveAsync(name);
        return Ok(res);
    }
}