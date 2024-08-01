using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.EF;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;

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
        public async Task<ActionResult<UserDTO>> GetUser(long id)
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

            await _userService.UpdateUser(user);
           
            return Ok(user);
        }
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.AddUser(user);

            return Ok(user);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDTO>> DeleteUser(long id)
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
