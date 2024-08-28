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
        //[HttpPost("add/{clerkId}")]
        //public async Task<ActionResult<UserDTO>> AddUser([FromRoute] string clerkId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    UserDTO user = await _userService.AddUser(clerkId);

        //    return Ok(user);
        //}

        [HttpPost("add")]
        public async Task<ActionResult<UserDTO>> AddUser([FromBody] AddUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             UserDTO user = await _userService.AddUser(request.data.id);

            return Ok(user);
           
        }

        //[HttpPost("add")]
        //public async Task<IActionResult> HandleWebhook()
        //{
        //    var WEBHOOK_SECRET = Environment.GetEnvironmentVariable("WEBHOOK_SECRET");

        //    if (string.IsNullOrEmpty(WEBHOOK_SECRET))
        //    {
        //        throw new Exception("Please add WEBHOOK_SECRET from Clerk Dashboard to your environment variables");
        //    }

        //    var headerPayload = Request.Headers;
        //    var svix_id = headerPayload["svix-id"];
        //    var svix_timestamp = headerPayload["svix-timestamp"];
        //    var svix_signature = headerPayload["svix-signature"];

        //    if (string.IsNullOrEmpty(svix_id) || string.IsNullOrEmpty(svix_timestamp) || string.IsNullOrEmpty(svix_signature))
        //    {
        //        return BadRequest("Error occurred -- no svix headers");
        //    }

        //    string body;
        //    using (var reader = new StreamReader(Request.Body))
        //    {
        //        body = await reader.ReadToEndAsync();
        //    }

        //    var wh = new Webhook(WEBHOOK_SECRET);

        //    try
        //    {
        //        var evt = wh.Verify(body, new Dictionary<string, string>
        //{
        //    { "svix-id", svix_id },
        //    { "svix-timestamp", svix_timestamp },
        //    { "svix-signature", svix_signature }
        //});

        //        if (evt.Type == "user.created")
        //        {
        //            var userId = evt.Data.Id;
        //            // Здесь вы можете добавить логику для сохранения userId в вашу базу данных
        //            Console.WriteLine($"New user created with ID: {userId}");
        //        }

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Error.WriteLine($"Error verifying webhook: {ex.Message}");
        //        return BadRequest("Error occurred");
        //    }
        //}

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
