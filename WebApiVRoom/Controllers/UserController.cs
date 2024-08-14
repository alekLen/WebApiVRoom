using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.EF;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Repositories;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {

            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }
        [HttpPut("update")]
        public async Task<ActionResult<UserDTO>> UpdateUser(UserDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserDTO user = await _userService.GetUser(u.Id);
            if (user == null)
            {
                return NotFound();
            }

            UserDTO usernew=await _userService.UpdateUser(u);

            return Ok(usernew);
        }
        [HttpPost("add/{clerk_id},{language},{country},{countryCode}")]
        public async Task<ActionResult<UserDTO>> AddUser(string clerk_id, string language, string country, string countryCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           UserDTO user = await _userService.AddUser(clerk_id, language,country, countryCode);

            return Ok(user);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDTO>> DeleteUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserDTO user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUser(id);

            return Ok(user);
        }

        [HttpGet("getbyclerkid/{clerk_id}")]
        public async Task<ActionResult<UserDTO>> ByClerkId(string clerk_id)
        {

            var user = await _userService.GetUserByClerkId(clerk_id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }
    }

   
}
