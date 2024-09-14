using Microsoft.AspNetCore.Mvc;

namespace Desafio.ProtocoloAPI.API.Controllers;

[ApiController]
public class MainController : ControllerBase
{
    protected IActionResult CustomResponse(bool success, object? result = null)
    {
        if (success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}
