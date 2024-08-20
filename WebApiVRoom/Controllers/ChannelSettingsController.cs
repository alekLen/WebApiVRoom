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
        public async Task<ActionResult<ChannelSettingsDTO>> GetChannelSettings(int id)
        {

            var ch = await _chService.GetChannelSettings(id);
            if (ch == null)
            {
                return NotFound();
            }
            return new ObjectResult(ch);
        }

        [HttpGet("getbyownerid/{ownerId}")]
        public async Task<ActionResult<ChannelSettingsDTO>> ByOwner(int ownerId)
        {

            var ch = await _chService.FindByOwner(ownerId);
            if (ch == null)
            {
                return NotFound();
            }
            return new ObjectResult(ch);
        }
        [HttpPut("update")]
        public async Task<ActionResult<ChannelSettingsDTO>> UpdateChannelSettings(ChannelSettingsDTO ch)
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<ChannelSettingsDTO>> DeleteChannelSettings(int id)
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
