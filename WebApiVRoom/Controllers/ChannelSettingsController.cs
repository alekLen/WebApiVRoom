using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelSettingsController : ControllerBase
    {
        private IChannelSettingsService _chService;

        public ChannelSettingsController(IChannelSettingsService chService)
        {
            _chService = chService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelSettingsDTO>> GetChannelSettings([FromRoute] int id)
        {

            var ch = await _chService.GetChannelSettings(id);
            if (ch == null)
            {
                return NotFound();
            }
            return new ObjectResult(ch);
        }

        [HttpGet("getbyownerid/{clerk_id}")]
        public async Task<ActionResult<ChannelSettingsDTO>> ByOwner([FromRoute] string clerk_id)
        {

            var ch = await _chService.FindByOwner(clerk_id);
            if (ch == null)
            {
                return NotFound();
            }
            return new ObjectResult(ch);
        }

        [HttpGet("getinfochannel/{clerkId}")]
        public async Task<ActionResult<ChannelUserFor_CommentDTO>> GetInfoChannelByClerkId([FromRoute] string clerkId)
        {
            var ch = await _chService.FindByOwner(clerkId);
            if (ch == null)
            {
                return NotFound();
            }

            ChannelUserFor_CommentDTO user = new() { 
                Clerk_Id=clerkId,
                ChannelBanner = ch.ChannelBanner,
                ChannelName = ch.ChannelName };

            return new ObjectResult(user);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ChannelSettingsDTO>> UpdateChannelSettings([FromBody] ChannelSettingsDTO ch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ChannelSettingsDTO chDto = await _chService.UpdateChannelSettings(ch);
            if (chDto == null)
            {
                return NotFound();
            }

            return Ok(chDto);
        }
        [HttpPut("setlanguage/{clerkid}/{local}")]
        public async Task<ActionResult<ChannelSettingsDTO>> SetLanguage([FromRoute] string clerkid, [FromRoute] string local)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ChannelSettingsDTO chDto = await _chService.SetLanguageToChannel(clerkid, local);
            if (chDto == null)
            {
                return NotFound();
            }

            return Ok(chDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ChannelSettingsDTO>> DeleteChannelSettings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ChannelSettingsDTO chDto=await _chService.DeleteChannelSettings(id);

            return Ok(chDto);
        }
    }
}
