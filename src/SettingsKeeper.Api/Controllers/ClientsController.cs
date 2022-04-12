using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SettingsKeeper.RabbitMQ.Abstract;

namespace SettingsKeeper.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController: ControllerBase
{
    private readonly IRabbitMqService _rabbitMqService;

    public ClientsController(IRabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    [HttpGet]
    public IActionResult GetAllClients()
    {
        return Ok();
    }

    [HttpGet]
    [Route("name")]
    public IActionResult RegisterClient(string name)
    {
        _rabbitMqService.CreateNewRabbitQueue(name);
        return Ok();
    }

    [HttpGet]
    [Route("message")]
    public IActionResult SendMessageToClient([FromQuery] string name, [FromQuery] string message)
    {
        _rabbitMqService.SendMessage(name, message);
        return Ok();
    }
}