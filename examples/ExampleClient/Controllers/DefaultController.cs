using Microsoft.AspNetCore.Mvc;

namespace ExampleClient.Controllers;

[ApiController]
[Route("")]
public class DefaultController: ControllerBase
{
    [HttpGet]
    public IActionResult SayHello()
    {
        return Ok("Hello");
    }
}