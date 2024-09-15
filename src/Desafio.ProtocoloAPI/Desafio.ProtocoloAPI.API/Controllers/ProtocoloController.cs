using Desafio.ProtocoloAPI.Application.Features.Protocolos.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Desafio.ProtocoloAPI.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProtocoloController : MainController
{
    private readonly IMediator _mediator;

    public ProtocoloController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SearchProtocoloQuery query)
    {
        var result = await _mediator.Send(query);

        if (result == null)
            return CustomResponse(false, null);

        return CustomResponse(true, result);
    }
}
