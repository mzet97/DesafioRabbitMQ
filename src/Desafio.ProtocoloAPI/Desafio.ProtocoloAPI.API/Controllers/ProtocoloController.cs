using Desafio.ProtocoloAPI.Application.Features.Protocolos.Queries;
using Desafio.ProtocoloAPI.Application.Features.Protocolos.ViewModels;
using Desafio.ProtocoloAPI.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Desafio.ProtocoloAPI.API.Controllers;

//[Authorize]
[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ProtocoloController : MainController
{
    private readonly IMediator _mediator;
    private readonly IDistributedCache _cache;

    public ProtocoloController(IMediator mediator, IDistributedCache cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    [HttpGet]
    //[OutputCache(Duration = 500)]
    public async Task<IActionResult> Get([FromQuery] SearchProtocoloQuery query)
    {
        string cacheKey = $"Protocolo_{query.ToString()}";
        var cachedData = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return Ok(JsonSerializer.Deserialize<BaseResult<ProtocolViewModel>>(cachedData));
        }

        var result = await _mediator.Send(query);

        var serializedWeather = JsonSerializer.Serialize(result);
        var cacheOptions = new DistributedCacheEntryOptions()
           .SetSlidingExpiration(TimeSpan.FromMinutes(500)) // Expiração deslizante
           .SetAbsoluteExpiration(TimeSpan.FromHours(5)); // Expiração absoluta

        await _cache.SetStringAsync(cacheKey, serializedWeather, cacheOptions);

        if (result == null)
            return CustomResponse(false, null);

        return CustomResponse(true, result);
    }

    [HttpGet("teste")]
    [OutputCache(Duration = 500)]
    public async Task<IActionResult> GetTeste([FromQuery] SearchProtocoloQuery query)
    {
        return Ok("OK");
    }
}
