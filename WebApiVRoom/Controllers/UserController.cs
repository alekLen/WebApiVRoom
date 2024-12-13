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
using static WebApiVRoom.BLL.DTO.AddUserRequest;
using WebApiVRoom.BLL.Helpers;

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
        private IEmailService _emailService;

        public UserController(IUserService userService,IVideoService video,INotificationService notificationService,
            IConfiguration configuration, IEmailService emailService)
        {
            _userService = userService;
            _notificationService = notificationService;
            _configuration = configuration;
            _videoService = video;
            _emailService = emailService;
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
                string SigningSecret = _configuration["Clerk:WebhookSecret"]; 
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
                    foreach (var item in request.data.email_addresses)
                    {
                        EmailDTO email = new EmailDTO();
                        email.EmailAddress=item.email_address;
                        email.UserClerkId=user.Clerk_Id;
                        if (item.id == request.data.primary_email_address_id)
                        {
                            email.IsPrimary = true;
                            //SendEmailHelper.SendEmailMessage(request.data.first_name + " " + request.data.last_name,
                            //    item.email_address, " Wellcome to VRoom! Your regestration is successful.");
                        }
                        else
                            email.IsPrimary = false;
                        await _emailService.AddEmail(email);
                    }
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
                    foreach (var item in request.data.email_addresses)
                    {
                        if (item.id == request.data.primary_email_address_id)
                        {
                            SendEmailHelper.SendEmailMessage(request.data.first_name + " " + request.data.last_name,
                                item.email_address, ", Your regestration on VRoom has been deleted. We are waiting for you back ");
                        }
                    }
                    UserDTO user2 = await _userService.Delete(request.data.id);              
                return Ok(user2);
            }
                if (request.type == "user.updated")
                {
                    IEnumerable<EmailDTO> ems = await _emailService.GetAllEmailsByUser(request.data.id);
                    foreach(var email in ems){
                        await _emailService.DeleteEmail(email.Id);
                    }
                    foreach (var item in request.data.email_addresses)
                    {
                        EmailDTO email = new EmailDTO();
                        email.EmailAddress = item.email_address;
                        email.UserClerkId = request.data.id;
                        if (item.id == request.data.primary_email_address_id)
                            email.IsPrimary = true;
                        else
                            email.IsPrimary = false;
                        await _emailService.AddEmail(email);
                    }
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

        [HttpPut("subscribeonmysubscriptionchannelactivity/{clerkId}/{subs}")]
        public async Task<ActionResult> SubscribeOnMySubscriptionChannelActivity([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.SubscribedOnMySubscriptionChannelActivity = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }

        [HttpPut("subscribeonactivityonmychannel/{clerkId}/{subs}")]
        public async Task<ActionResult> SubscribeOnActivityOnMyChannel([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.SubscribedOnActivityOnMyChannel = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("subscribeonrecomendedvideo/{clerkId}/{subs}")]
        public async Task<ActionResult> SubscribeOnRecomendedVideo([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.SubscribedOnRecomendedVideo = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("subscribeonactivityonmycomments/{clerkId}/{subs}")]
        public async Task<ActionResult> SubscribeOnActivityOnMyComments([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.SubscribedOnOnActivityOnMyComments = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("subscribeonothersmentiononmychannel/{clerkId}/{subs}")]
        public async Task<ActionResult> SubscribeOnOthersMentionOnMyChannel([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.SubscribedOnOthersMentionOnMyChannel = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("subscribeonsharemycontent/{clerkId}/{subs}")]
        public async Task<ActionResult> SubscribeOnShareMyContent([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.SubscribedOnShareMyContent = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("subscribeonpromotionalcontent/{clerkId}/{subs}")]
        public async Task<ActionResult> SubscribeOnPromotionalContent([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.SubscribedOnPromotionalContent = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }

        [HttpPut("emailsubscribeonmysubscriptionchannelactivity/{clerkId}/{subs}")]
        public async Task<ActionResult> EmailSubscribeOnMySubscriptionChannelActivity([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.EmailSubscribedOnMySubscriptionChannelActivity = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }

        [HttpPut("emailsubscribeonactivityonmychannel/{clerkId}/{subs}")]
        public async Task<ActionResult> EmailSubscribeOnActivityOnMyChannel([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.EmailSubscribedOnActivityOnMyChannel = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("emailsubscribeonrecomendedvideo/{clerkId}/{subs}")]
        public async Task<ActionResult> EmailSubscribeOnRecomendedVideo([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.EmailSubscribedOnRecomendedVideo = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("emailsubscribeonactivityonmycomments/{clerkId}/{subs}")]
        public async Task<ActionResult> EmailSubscribeOnActivityOnMyComments([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.EmailSubscribedOnOnActivityOnMyComments = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("emailsubscribeonothersmentiononmychannel/{clerkId}/{subs}")]
        public async Task<ActionResult> EmailSubscribeOnOthersMentionOnMyChannel([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.EmailSubscribedOnOthersMentionOnMyChannel = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("emailsubscribeonsharemycontent/{clerkId}/{subs}")]
        public async Task<ActionResult> EmailSubscribeOnShareMyContent([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.EmailSubscribedOnShareMyContent = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("emailsubscribeonpromotionalcontent/{clerkId}/{subs}")]
        public async Task<ActionResult> EmailSubscribeOnPromotionalContent([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.EmailSubscribedOnPromotionalContent = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }
        [HttpPut("subscribeonmainemailnotifications/{clerkId}/{subs}")]
        public async Task<ActionResult> SubscribedOnMainEmailNotifications([FromRoute] string clerkId, [FromRoute] bool subs)
        {

            var user = await _userService.GetUserByClerkId(clerkId);
            if (user == null)
            {
                return NotFound();
            }
            user.SubscribedOnMainEmailNotifications = subs;
            await _userService.UpdateUser(user);
            return Ok();
        }

    }
        
}
