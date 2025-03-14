using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdController : ControllerBase
{
    private readonly IAdService _adService;

    public AdController(IAdService adService)
    {
        _adService = adService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginated([FromQuery] int page, [FromQuery] int perPage, [FromQuery] string? searchQuery)
    {
        var ads = await _adService.GetPaginated(page, perPage, searchQuery);
        var count = await _adService.Count(searchQuery);
        return Ok(new {ads, count});
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(AdDTO adDTO)
    {
        var ad = await _adService.Add(adDTO);
        return Ok(ad);
    }
    
    [HttpGet("getrandom")]
    public async Task<IActionResult> GetRandom()
    {
        var ad = await _adService.GetRandom();
        return Ok(ad);
    }
}