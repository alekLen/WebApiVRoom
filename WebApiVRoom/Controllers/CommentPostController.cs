using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.Helpers;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentPostController : Controller
    {
        private ICommentPostService _comService;
        private ILikesDislikesCPService _likesService;
        private readonly IHubContext<ChatHub> _hubContext;

        public CommentPostController(ICommentPostService cService, ILikesDislikesCPService likesService, IHubContext<ChatHub> hubContext)
        {
            _comService = cService;
            _likesService = likesService;
            _hubContext = hubContext;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CommentPostDTO>> GetComment([FromRoute] int id)
        {

            var c = await _comService.GetCommentPost(id);
            if (c == null)
            {
                return NotFound();
            }
            return new ObjectResult(c);
        }
        [HttpPut("update")]
        public async Task<ActionResult<CommentPostDTO>> UpdateCommentPost([FromBody] CommentPostDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CommentPostDTO ans = await _comService.GetCommentPost(u.Id);
            if (ans == null)
            {
                return NotFound();
            }

            CommentPostDTO c = await _comService.UpdateCommentPost(u);
            object com = ConvertObject(c);

            //await WebSocketHelper.SendMessageToAllAsync("update_commentpost", com);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "update_commentpost", payload = com });

            return Ok(c);
        }

        [HttpPost("add")]
        public async Task<ActionResult<CommentPostDTO>> Add([FromBody] CommentPostDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CommentPostDTO ans = await _comService.AddCommentPost(request);
            object com = ConvertObject(ans);

            //await WebSocketHelper.SendMessageToAllAsync("new_commentpost", com);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "new_commentpost", payload = com });

            return Ok(ans);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CommentPostDTO>> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CommentPostDTO ans = await _comService.GetCommentPost(id);
            if (ans == null)
            {
                return NotFound();
            }

            await _comService.DeleteCommentPost(id);

            return Ok(ans);
        }

        [HttpGet("getbyuserid/{user_id}")]
        public async Task<ActionResult<List<CommentPostDTO>>> ByUserId([FromRoute] int user_id)
        {

            List<CommentPostDTO > list = await _comService.GetByUser(user_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }
        [HttpGet("getbypostid/{post_id}")]
        public async Task<ActionResult<CommentPostDTO>> ByPostId([FromRoute] int post_id)
        {

            List<CommentPostDTO> list = await _comService.GetCommentPostsByPost(post_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }
        [HttpGet("getbypostwithpagination/{pageNumber}/{pageSize}/{post_id}")]
        public async Task<ActionResult<CommentPostDTO>> ByPostIdPaginated([FromRoute] int pageNumber, [FromRoute] int pageSize, [FromRoute] int post_id)
        {

            List<CommentPostDTO> list = await _comService.GetByPostPaginated(pageNumber, pageSize,post_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }

        [HttpPut("like/{comment}/{user}/{i}")]
        public async Task<ActionResult> likeCommentPost([FromRoute] int comment, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesCPDTO like = await _likesService.Get(comment, user);
            if (like == null && user != i)
            {
                LikesDislikesCPDTO likeDto = new() { commentId = comment, userId = user };
                await _likesService.Add(likeDto);
                CommentPostDTO ans = await _comService.GetCommentPost(comment);
                if (ans == null)
                {
                    return NotFound();
                }
                ans.LikeCount += 1;

                CommentPostDTO c = await _comService.UpdateCommentPost(ans);

                //await WebSocketHelper.SendMessageToAllAsync("like_commentpost", null);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "like_commentpost", payload = c });

                return Ok();
            }

            return Ok();
        }
        [HttpPut("dislike/{comment}/{user}/{i}")]
        public async Task<ActionResult> dislikeCommentPost([FromRoute] int comment, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesCPDTO like = await _likesService.Get(comment, user);
            if (like == null && user != i)
            {
                LikesDislikesCPDTO likeDto = new() { commentId = comment, userId = user };
                await _likesService.Add(likeDto);
                CommentPostDTO ans = await _comService.GetCommentPost(comment);
                if (ans == null)
                {
                    return NotFound();
                }
                ans.DislikeCount += 1;

                CommentPostDTO c = await _comService.UpdateCommentPost(ans);

                //await WebSocketHelper.SendMessageToAllAsync("dislike_commentpost", null);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "dislike_commentpost", payload = c });

                return Ok();
            }
            return Ok();
        }

        [HttpPut("topin/{comment}")]
        public async Task<ActionResult> pinCommentPost([FromRoute] int comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CommentPostDTO ans = await _comService.GetCommentPost(comment);
            if (ans == null)
            {
                return NotFound();
            }

            ans.IsPinned = true;

            CommentPostDTO c = await _comService.UpdateCommentPost(ans);
            object com = ConvertObject(c);
            //await WebSocketHelper.SendMessageToAllAsync("pin_commentpost", com);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "pin_commentpost", payload = com });

            return Ok();
        }
        [HttpPut("unpin/{comment}")]
        public async Task<ActionResult> unpinCommentVideo([FromRoute] int comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CommentPostDTO ans = await _comService.GetCommentPost(comment);
            if (ans == null)
            {
                return NotFound();
            }

            ans.IsPinned = false;

            CommentPostDTO c = await _comService.UpdateCommentPost(ans);
            object com = ConvertObject(c);
            //await WebSocketHelper.SendMessageToAllAsync("pin_commentpost",com);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "pin_commentpost", payload = com });

            return Ok();
        }

        private object ConvertObject(CommentPostDTO ans)
        {
            object obj = new
            {
                id = ans.Id,
                text = ans.Comment,
                isEdited = ans.IsEdited,
                isPinned = ans.IsPinned,
            };
            return obj;
        }
    }
}
