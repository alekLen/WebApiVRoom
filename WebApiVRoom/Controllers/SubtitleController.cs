using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubtitleController : Controller
    {
        private ISubtitleService _subService;

        public SubtitleController(ISubtitleService subService)
        {
            _subService = subService;
        }

        [HttpGet("getpublishsubtitles/{videoid}")]
        public async Task<ActionResult<List<SubtitleDTO>>> GetPublishedSubtitlesByVideo(int videoid)
        {
            var subs = await _subService.GetPublishedSubtitlesByVideo(videoid);
           
            return new ObjectResult(subs);
        }

        [HttpGet("getnotpublishsubtitles/{videoid}")]
        public async Task<ActionResult<List<SubtitleDTO>>> GetNotPublishedSubtitlesByVideo(int videoid)
        {
            var subs = await _subService.GetNotPublishedSubtitlesByVideo(videoid);

            return new ObjectResult(subs);
        }


        [HttpPost("add")]
        public async Task<ActionResult> AddSubtitle(IFormFile fileVTT, [FromForm] SubtitleDTO sub)
        {
            
            await _subService.AddSubtitle(sub, fileVTT);

            return Ok();
        }

        [HttpPut("update")]
        public async Task<ActionResult<SubtitleDTO>> UpdateSubtitle(IFormFile fileVTT, string code, string name,
            string videoId, string publish,string id)
        {
            
            SubtitleDTO s = await _subService.GetSubtitle(int.Parse(id));
            if (s == null)
            {
                return NotFound();
            }
            bool p = false;
            if (publish == "yes")
                p = true;
            var subtitleDTO = new SubtitleDTO
            {
                Id= int.Parse(id),
                LanguageName = name,
                LanguageCode = code,
                VideoId = int.Parse(videoId),
                IsPublished = p
            };

            SubtitleDTO sub_new = await _subService.UpdateSubtitle(subtitleDTO, fileVTT);

            return Ok(sub_new);
        }
    }
}
