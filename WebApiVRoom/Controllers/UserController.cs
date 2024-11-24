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
using System.Net;
using Svix;
using Svix.Exceptions;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserService _userService;
        private IVideoService _videoService;
        private INotificationService _notificationService;
        private IConfiguration _configuration;

        public UserController(IUserService userService,IVideoService video,INotificationService notificationService, IConfiguration configuration)
        {
            _userService = userService;
            _notificationService = notificationService;
            _configuration = configuration;
            _videoService = video;
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
        public async Task<ActionResult<UserDTO>> AddUser()
        {
            try
            {
                string SigningSecret = _configuration["Clerk:WebhookSecret"]; //  секретный ключ
                Request.EnableBuffering();

                string requestBody;
                using (var reader = new StreamReader(Request.Body, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    Request.Body.Position = 0;
                }

                var headers = new WebHeaderCollection();
                headers.Set("svix-id", Request.Headers["svix-id"]);
                headers.Set("svix-timestamp", Request.Headers["svix-timestamp"]);
                headers.Set("svix-signature", Request.Headers["svix-signature"]);
          
                var wh= new Webhook(SigningSecret);
           
                wh.Verify(requestBody,headers);
           
                var request = JsonConvert.DeserializeObject<AddUserRequest>(requestBody);
           
                if (request.type == "user.created")
                {
                    UserDTO user = await _userService.AddUser(request.data.id,request.data.image_url);
                        await AddNotification(user, "Добро пожаловать на на сайт!");
                    return Ok(user);             
                }

                if (request.type == "user.deleted")
                {
                        UserDTO user = await _userService.GetUserByClerkId(request.data.id);

                        List<VideoDTO> videos = await _videoService.GetByChannelId(user.ChannelSettings_Id);
                        foreach (VideoDTO video in videos)
                        {
                            await _videoService.DeleteVideo(video.Id);
                        }
                    UserDTO user2 = await _userService.Delete(request.data.id);              
                    return Ok(user2);
                }
            }
            catch (Exception ex) { return BadRequest(ModelState); }

            return BadRequest(ModelState);
        }

        private async Task AddNotification(UserDTO user,string text)
        {
            NotificationDTO notification = new NotificationDTO();
            notification.Date = DateTime.Now;
            notification.UserId = user.Id;
            notification.IsRead = false;
            notification.Message = text;
            NotificationDTO n = await _notificationService.Add(notification);
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
            List<VideoDTO> videos = await _videoService.GetByChannelId(user.ChannelSettings_Id);
            foreach (VideoDTO video in videos)
            {
                await _videoService.DeleteVideo(video.Id);
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
        [HttpGet("getbyvideoid/{videoId}")]
        public async Task<ActionResult<UserDTO>> ByVideoId([FromRoute] int videoId)
        {

            var user = await _userService.GetUserByVideoId(videoId);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }

        [HttpGet("getbypostid/{postId}")]
        public async Task<ActionResult<UserDTO>> ByPostId([FromRoute] int postId)
        {

            var user = await _userService.GetUserByPostId(postId);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }


    }
     

   
}
