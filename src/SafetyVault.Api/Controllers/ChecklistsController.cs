using Microsoft.AspNetCore.Mvc;

namespace SafetyVault.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChecklistsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok("Es funktioniert!");
    }
}
