using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SettingsKeeper.Core.Abstract;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController: ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        var result = await _clientsService.GetAllClients();
        return Ok(result);
    }

    [HttpGet]
    [Route("{name}")]
    public IActionResult RegisterClient(string name)
    {
        _clientsService.AddNewClient(name);
        return Ok();
    }

    [HttpPost]
    public IActionResult SendMessageToClient([FromQuery] string name, [FromBody] JsonDocument message)
    {
        _clientsService.SendMessageToClient(name, message);
        return NoContent();
    }
}