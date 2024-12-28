using Microsoft.AspNetCore.Http.HttpResults;
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
        private IUserService _uService;

        public ChannelSettingsController(IChannelSettingsService chService, IUserService userService)
        {
            _chService = chService;
            _uService = userService;
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
        [HttpGet("getinfobychannelid/{id}")]
        public async Task<ActionResult<ChannelSettingsDTO>> GetChannelInfo([FromRoute] int id)
        {
            try
            {
                var ch = await _chService.GetChannelSettings(id);
                string name = ch.ChannelName;
                if (ch == null)
                {
                    return NotFound();
                }
                UserDTO u = await _uService.GetUser(ch.Owner_Id);
                if (ch.ChannelNikName != null)
                {
                    name = ch.ChannelNikName;
                }
                ChannelUserFor_CommentDTO user = new()
                {
                    Clerk_Id = u.Clerk_Id,
                    Channel_Id = ch.Id,
                    ChannelBanner = ch.ChannelBanner,
                    ChannelName = ch.ChannelName,
                    ChannelNikName = name,
                    ChannelProfilePhoto = ch.ChannelProfilePhoto,
                    Channel_URL = ch.Channel_URL,

                };
                return new ObjectResult(user);
            }
            catch (Exception ex) { return BadRequest(ModelState); }
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
            try
            {
                var ch = await _chService.FindByOwner(clerkId);
                string name = ch.ChannelName;
                if (ch == null)
                {
                    return NotFound();
                }
                if (ch.ChannelNikName != null)
                {
                    name = ch.ChannelNikName;
                }
                ChannelUserFor_CommentDTO user = new()
                {
                    Clerk_Id = clerkId,
                    Channel_Id = ch.Id,
                    ChannelBanner = ch.ChannelBanner,
                    ChannelName = ch.ChannelName,
                    ChannelNikName = name,
                    ChannelProfilePhoto = ch.ChannelProfilePhoto,
                    Channel_URL = ch.Channel_URL,
                };

                return new ObjectResult(user);
            }
            catch (Exception ex) { return BadRequest(ModelState); }
        }

        [HttpGet("gotochannel/{url}")]
        public async Task<ActionResult<ChannelSettingsDTO>> GetInfoChannelByURL([FromRoute] string url)
        {
            try
            {
                var ch = await _chService.GetByUrl(url);
                if (ch == null)
                {
                    return NotFound();
                }


                return new ObjectResult(ch);
            }
            catch (Exception ex) { return BadRequest(ModelState); }
        }

        [HttpPut("update")]
        public async Task<ActionResult<ChannelSettingsDTO>> UpdateChannelSettings(IFormFile? channelBanner, IFormFile? profilePhoto, [FromForm] ChannelSettingsDTO ch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FormFileCollection files = new FormFileCollection();
            files.Add(channelBanner);
            files.Add(profilePhoto);

            ChannelSettingsDTO chDto = await _chService.UpdateChannelSettings(ch, files);
            if (chDto == null)
            {
                return NotFound();
            }

            return Ok(chDto);
        }

        [HttpPut("updateShort")]
        public async Task<IActionResult> UpdateChannelSettingsShort(IFormFile? channelBanner, IFormFile? profilePhoto, [FromForm] ChannelSettingsShortDTO ch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FormFileCollection files = new FormFileCollection();
            files.Add(channelBanner);
            files.Add(profilePhoto);

            ChannelSettingsDTO chDto = await _chService.UpdateChannelSettingsShort(ch, files);
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

            ChannelSettingsDTO chDto = await _chService.DeleteChannelSettings(id);

            return Ok(chDto);
        }

        [HttpGet("checknicknameunique/{nickName}/{chSettingsId}")]
        public async Task<IActionResult> CheckNickNameUnique([FromRoute] string nickName, [FromRoute] int chSettingsId)
        {
            bool isUnique = await _chService.IsNickNameUnique(nickName, chSettingsId);
            return new ObjectResult(new { isUnique });
        }
    }
}
