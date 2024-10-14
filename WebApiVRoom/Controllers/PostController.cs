using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiVRoom.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private IPostService _postService;
        private ILikesDislikesPService _likesService;
        private readonly IHubContext<ChatHub> _hubContext;


        public PostController(IPostService postService, ILikesDislikesPService likesService, IHubContext<ChatHub> hubContext)
        {
            _postService = postService;
            _likesService = likesService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPosts()
        {
            return new ObjectResult(await _postService.GetAllPosts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(int id)
        {
            var post = await _postService.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            return new ObjectResult(post);
        }

        [HttpGet("getbychannelid/{channel_id}")]
        public async Task<ActionResult<PostDTO>> GetPostByChannellId(int channel_id)
        {
            var post = await _postService.GetPostByChannellId(channel_id);
            if (post == null)
            {
                return NotFound();
            }
            return new ObjectResult(post);
        }

        [HttpGet("getbyposttext/{postText}")]
        public async Task<ActionResult<PostDTO>> GetByPostText(string postText)
        {
            var post = await _postService.GetPostByText(postText);
            if (post == null)
            {
                return NotFound();
            }
            return new ObjectResult(post);
        }

        [HttpPost("add")]
        public async Task<ActionResult<PostDTO>> AddPost(IFormFile? img, IFormFile? video, [FromForm] AddPostRequest req)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           PostDTO post = await _postService.AddPost(img,video,req.text,req.id);
            object obj = ConvertObject(post);
            //await WebSocketHelper.SendMessageToAllAsync("new_post", obj);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "new_post", payload = obj });
            return Ok();
        }

        [HttpPut("update")]
        public async Task<ActionResult<PostDTO>> UpdatePost(PostDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PostDTO post = await _postService.GetPost(u.Id);
            if (post == null)
            {
                return NotFound();
            }

            PostDTO post_new = await _postService.UpdatePost(u);

            return Ok(post_new);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PostDTO>> DeletePost(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PostDTO post = await _postService.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }

            await _postService.DeletePost(id);

            object obj = ConvertObject(post);

            //await WebSocketHelper.SendMessageToAllAsync("post_deleted", obj);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "post_deleted", payload = obj });
        

            return Ok(post);
        }
        [HttpPut("like/{post}/{user}/{i}")]
        public async Task<ActionResult> likePost([FromRoute] int post, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesPDTO like = await _likesService.Get(post, user);
            if (like == null && user != i)
            {
                LikesDislikesPDTO likeDto = new() { postId = post, userId = user };
                await _likesService.Add(likeDto);
                PostDTO ans = await _postService.GetPost(post);
                if (ans == null)
                {
                    return NotFound();
                }
                ans.LikeCount += 1;

                PostDTO c = await _postService.UpdatePost(ans);
                object obj = ConvertObject(c);
                //await WebSocketHelper.SendMessageToAllAsync("new_likepost", obj);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "new_likepost", payload = obj });

                return Ok();
            }

            return Ok();
        }
        [HttpPut("dislike/{post}/{user}/{i}")]
        public async Task<ActionResult> dislikePost([FromRoute] int post, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesPDTO like = await _likesService.Get(post, user);
            if (like == null && user != i)
            {
                LikesDislikesPDTO likeDto = new() { postId = post, userId = user };
                await _likesService.Add(likeDto);
                PostDTO ans = await _postService.GetPost(post);
                if (ans == null)
                {
                    return NotFound();
                }
                ans.DislikeCount += 1;

                PostDTO c = await _postService.UpdatePost(ans);
                object obj = ConvertObject(c);

                //await WebSocketHelper.SendMessageToAllAsync("new_dislikepost", obj);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "new_dislikepost", payload = obj });

                return Ok();
            }
            return Ok();
        }

        private object ConvertObject(PostDTO ans)
        {
            object obj = new
            {
                id = ans.Id,
                text = ans.Text,
                channelSettingsId = ans.ChannelSettingsId,
                date= ans.Date,
                photo=ans.Photo,
                video=ans.Video,
                likeCount=ans.LikeCount,
                dislikeCount=ans.DislikeCount
            };
            return obj;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public int ChannelSettingsId { get; set; }
        public DateTime Date { get; set; }
        public string? Photo { get; set; }
        public string? Video { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

    }
}
