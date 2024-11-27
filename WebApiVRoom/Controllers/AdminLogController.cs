namespace WebApiVRoom.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class AdminLogController : Controller
{
    private readonly IAdminLogService _adminLogService;

    public AdminLogController(IAdminLogService adminLogService)
    {
        _adminLogService = adminLogService;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<AdminLogDTO>> GetAdminLog(int id)
    {
        var adminLog = await _adminLogService.GetById(id);
        if (adminLog == null)
        {
            return NotFound();
        }
        return new ObjectResult(adminLog);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdminLogDTO>>> GetAdminLogs([FromQuery] int page, [FromQuery] int perPage, [FromQuery] string type, [FromQuery] string? searchQuery)
    {
        var adminLogs = await _adminLogService.GetPaginatedAndSortedWithQuery(page, perPage, type, searchQuery);
        return new ObjectResult(adminLogs);
    }
}