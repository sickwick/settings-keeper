using Microsoft.AspNetCore.Mvc;
using SettingsKeeper.Cache.Abstract;
using SettingsKeeper.Core.Abstract;
using SettingsKeeper.MongoDb.Abstract;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("")]
public class SettingsController:ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly IRedisProvider _cacheProvider;
    private readonly IMongoDbProvider _mongoDbProvider;

    public SettingsController(
        ISettingsService settingsService, 
        IRedisProvider cacheProvider,
        IMongoDbProvider mongoDbProvider)
    {
        _settingsService = settingsService;
        _cacheProvider = cacheProvider;
        _mongoDbProvider = mongoDbProvider;
    }

    [HttpGet]
    [Route("{name}")]
    public async Task<IActionResult> GetSetting(string name)
    {
        return Ok();
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