using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentReportController : ControllerBase
    {
        private readonly IContentReportService _contentReportService;

        public ContentReportController(IContentReportService contentReportService)
        {
            _contentReportService = contentReportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginated(int page, int perPage, string? searchQuery)
        {
            var contentReports = await _contentReportService.GetPaginated(page, perPage, searchQuery);
            var count = await _contentReportService.Count(searchQuery);
            return Ok(new {contentReports, count});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contentReport = await _contentReportService.GetById(id);
            return Ok(contentReport);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContentReportDTO contentReportDTO)
        {
            var contentReport = await _contentReportService.Add(contentReportDTO);
            return Ok(contentReport);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ContentReportDTO contentReportDTO)
        {
            var contentReport = await _contentReportService.Update(contentReportDTO);
            return Ok(contentReport);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contentReportService.Delete(id);
            return Ok();
        }
        
        [HttpPut("{id}/{adminId}/process")]
        public async Task<IActionResult> Process([FromRoute] int id, [FromRoute] string adminId)
        {
            await _contentReportService.Process(id, adminId);
            return Ok();
        }

    }
}
