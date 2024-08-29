using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.DAL.EF;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Repositories;
using Microsoft.AspNetCore;
using Microsoft.OpenApi.Any;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserService _userService;
        //var WEBHOOK_SECRET = Configuration["Clerk:WebhookSecret

       

        public UserController(IUserService userService)
        {
            _userService = userService;
           
        }
        

            [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser([FromRoute] int id)
        {

            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }
        [HttpPut("update")]
        public async Task<ActionResult<UserDTO>> UpdateUser([FromBody] UserDTO u)
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

        [HttpPost("add")]
        public async Task<ActionResult<UserDTO>> AddUser([FromBody] AddUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (request.type == "user.created")
            {
                UserDTO user = await _userService.AddUser(request.data.id);
                request = null;
                return Ok(user);
              
            }
            if (request.type == "user.deleted")
            {
                UserDTO user = await _userService.Delete(request.data.id);
                request = null;
                return Ok(user);
            }
            return BadRequest(ModelState);
        }

     

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDTO>> DeleteUser([FromRoute] int id)
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

        [HttpGet("getbyclerkid/{clerkId}")]
        public async Task<ActionResult<UserDTO>> ByClerkId([FromRoute] string clerkId)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }
    }

   
}
