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
        //private readonly VRoomContext _context;
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
        [HttpPut]
        public async Task<ActionResult<UserDTO>> PutUser(UserDTO u)
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

            //await _userService.UpdateUser(user);

            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(string clerk_id, string language, string country, string countryCode)
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

            //_userService.DeleteUser(user);

            return Ok(user);
        }


    }

   
}
