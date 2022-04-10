using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SettingsKeeper.Api.Models;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.MongoDb.Abstract;
using SettingsKeeper.MongoDb.Models;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("")]
public class SettingsController:ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly ISettingsKeeperCacheProvider _cacheProvider;
    private readonly IMongoDbProvider _mongoDbProvider;

    public SettingsController(
        ISettingsService settingsService, 
        ISettingsKeeperCacheProvider cacheProvider,
        IMongoDbProvider mongoDbProvider)
    {
        _settingsService = settingsService;
        _cacheProvider = cacheProvider;
        _mongoDbProvider = mongoDbProvider;
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<IActionResult> GetSetting(string name,[FromServices] IOptions<MongoSettings> mongo)
    {
        await _cacheProvider.SetAsync(name, $"{name}-{name}");
        var res = await _cacheProvider.GetAsync(name);
        var db = _mongoDbProvider.GetMongoClient(mongo);
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