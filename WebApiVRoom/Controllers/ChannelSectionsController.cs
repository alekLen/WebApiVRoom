using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using static System.Collections.Specialized.BitVector32;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ChannelSectionsController : ControllerBase
    {
        private IChannelSettingsService _chService;
        private IChannelSectionsService _chsService;

        public ChannelSectionsController(IChannelSettingsService chService, IChannelSectionsService chsService)
        {
            _chService = chService;
            _chsService = chsService;
        }


        [HttpGet("available/{channelOwnerId}")]
        public async Task<IActionResult> GetAvailableChannelSectionsByClerkId(string channelOwnerId)
        {
            var sections = await _chsService.GetAvailableChannelSectionsByChannelOwnerId(channelOwnerId);
            return Ok(sections);
        }

        [HttpGet("user/{channelOwnerId}")]
        public async Task<IActionResult> GetUserSections(string channelOwnerId)
        {
            var ownCh = await _chService.FindByOwner(channelOwnerId);
            var userSections = await _chsService.GetChannelSectionsAsync(ownCh.Id);
            return Ok(userSections);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateChannelSections([FromQuery] string clerkId, [FromBody] List<ChannelSectionDTO> chs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _chsService.UpdateRangeChannelSectionsByClerkId(clerkId, chs);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelSettingsDTO>> GetById([FromRoute] int id)
        {

            var chs = await _chsService.GetChSectionById(id);
            if (chs == null)
            {
                return NotFound();
            }
            return new ObjectResult(chs);
        }


        [HttpGet("getbyglobalchannelsectionbyname/{globChSectionName}")]
        public async Task<ActionResult<ChSectionDTO>> GetByChannelNikName([FromRoute] string globChSectionName)
        {
            try
            {
                var chs = await _chsService.GetChSectionByTitle(globChSectionName);
                if (chs == null)
                {
                    return NotFound();
                }

                return new ObjectResult(chs);
            }
            catch (Exception ex) { return BadRequest(ModelState); }
        }

        [HttpPost("addglobalchsection")]
        public async Task<ActionResult<ChSectionDTO>> AddChSettings([FromForm] ChSectionDTO chs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ChSectionDTO chSection = await _chsService.UpdateChSection(chs);


            return Ok(chSection);
        }

        [HttpPut("updateglobalchsection")]
        public async Task<ActionResult<ChSectionDTO>> UpdateChSettings([FromForm] ChSectionDTO chs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ChSectionDTO chSec = await _chsService.GetChSectionById(chs.Id);
            if (chSec == null)
            {
                return NotFound();
            }

            ChSectionDTO chSection = await _chsService.UpdateChSection(chs);
           

            return Ok(chSection);
        }


        [HttpDelete("deleteglobalchsection/{id}")]
        public async Task<ActionResult<ChannelSettingsDTO>> DeleteChannelSettings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ChSectionDTO chSection = await _chsService.GetChSectionById(id);
            if (chSection == null)
            {
                return NotFound();
            }
            await _chsService.DeleteChSection(id);

            return Ok();
        }

    }
}
