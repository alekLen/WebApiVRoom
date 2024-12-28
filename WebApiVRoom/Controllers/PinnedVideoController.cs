using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PinnedVideoController : ControllerBase
    {
        private IPinnedVideoService _pinnedVideoService;

        public PinnedVideoController(IPinnedVideoService pinnedVideoService)
        {
            _pinnedVideoService = pinnedVideoService;
        }

        

        [HttpGet("{id}")]
        public async Task<ActionResult<PinnedVideoDTO>> GetPinnedVideoById(int id)
        {
            var pinnedVideo = await _pinnedVideoService.GetPinnedVideoById(id);
            if (pinnedVideo == null)
            {
                return NotFound();
            }
            return new ObjectResult(pinnedVideo);
        }

        [HttpGet("getispinnedvideobychannelid/{channelId}")]
        public async Task<ActionResult<PinnedVideoDTO>> GetIsPinnedVideoByChannelId(int channelId)
        {
            var pinnedVideo = await _pinnedVideoService.GetPinnedVideoByChannelId(channelId);
            if (pinnedVideo == null)
            {
                return new ObjectResult(null);
            }
            return new ObjectResult(pinnedVideo);
        }

        [HttpGet("getpinnedvideobychannelid/{channelId}")]
        public async Task<ActionResult<PinnedVideoDTO>> GetPinnedVideoByChannelId(int channelId)
        {
            var pinnedVideo = await _pinnedVideoService.GetPinnedVideoByChannelId(channelId);
            if (pinnedVideo == null)
            {
                return NotFound();
            }
            return new ObjectResult(pinnedVideo);
        }


        [HttpPost("add")]
        public async Task<ActionResult<PinnedVideoDTO>> AddPinnedVideo(PinnedVideoDTO pinnedVideoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _pinnedVideoService.AddPinnedVideo(pinnedVideoDTO);
            return Ok();
        }



        [HttpPut("update")]
        public async Task<ActionResult<PinnedVideoDTO>> UpdateTag(PinnedVideoDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PinnedVideoDTO pinnedVideo = await _pinnedVideoService.GetPinnedVideoById(u.Id);
            if (pinnedVideo == null)
            {
                return NotFound();
            }

            PinnedVideoDTO pinnedVideo_new = await _pinnedVideoService.UpdatePinnedVideo(u);

            return Ok(pinnedVideo_new);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PinnedVideoDTO>> DeletePinnedVideo(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PinnedVideoDTO pinnedVideo = await _pinnedVideoService.GetPinnedVideoById(id);
            if (pinnedVideo == null)
            {
                return NotFound();
            }

            await _pinnedVideoService.DeletePinnedVideo(id);

            return Ok(pinnedVideo);
        }
    }
}
