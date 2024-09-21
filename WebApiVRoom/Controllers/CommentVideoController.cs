using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.Helpers;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentVideoController : Controller
    {
        private ICommentVideoService _comService;
        private ILikesDislikesCVService _likesService;

        public CommentVideoController(ICommentVideoService cService, ILikesDislikesCVService likesService)
        {
            _comService = cService;
            _likesService = likesService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CommentVideoDTO>> GetComment([FromRoute] int id)
        {

            var c = await _comService.GetCommentVideoById(id);
            if (c == null)
            {
                return NotFound();
            }
            return new ObjectResult(c);
        }
        [HttpPut("update")]
        public async Task<ActionResult<CommentVideoDTO>> UpdateCommentVideo([FromBody] CommentVideoDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CommentVideoDTO ans = await _comService.GetCommentVideoById(u.Id);
            if (ans == null)
            {
                return NotFound();
            }

            CommentVideoDTO c = await _comService.UpdateCommentVideo(u);

            await WebSocketHelper.SendMessageToAllAsync("new_comment", null);

            return Ok(c);
        }
        [HttpPut("like/{comment}/{user}/{i}")]
        public async Task<ActionResult> likeCommentVideo([FromRoute] int comment, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesCVDTO like = await _likesService.Get(comment, user);
            if (like == null && user!=i )
            {
                LikesDislikesCVDTO likeDto = new() { commentId=comment, userId=user };
                await _likesService.Add(likeDto);
            CommentVideoDTO ans = await _comService.GetCommentVideoById(comment);
            if (ans == null)
            {
                return NotFound();
            }
            ans.LikeCount += 1;

            CommentVideoDTO c = await _comService.UpdateCommentVideo(ans);

                await WebSocketHelper.SendMessageToAllAsync("new_comment", null);

                return Ok();
            }

            return Ok();
        }
        [HttpPut("dislike/{comment}/{user}/{i}")]
        public async Task<ActionResult> dislikeCommentVideo([FromRoute] int comment, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesCVDTO like = await _likesService.Get(comment, user);
            if (like == null && user != i)
            {
                LikesDislikesCVDTO likeDto = new() { commentId = comment, userId = user };
                await _likesService.Add(likeDto);
                CommentVideoDTO ans = await _comService.GetCommentVideoById(comment);
                if (ans == null)
                {
                    return NotFound();
                }
                ans.DislikeCount += 1;

                CommentVideoDTO c = await _comService.UpdateCommentVideo(ans);

                await WebSocketHelper.SendMessageToAllAsync("new_comment", null);

                return Ok();
            }
            return Ok();
        }

        [HttpPut("topin/{comment}")]
        public async Task<ActionResult> pinCommentVideo([FromRoute] int comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
                CommentVideoDTO ans = await _comService.GetCommentVideoById(comment);
                if (ans == null)
                {
                    return NotFound();
                }

                ans.IsPinned=true;

                CommentVideoDTO c = await _comService.UpdateCommentVideo(ans);

                await WebSocketHelper.SendMessageToAllAsync("new_comment", null);
             
            return Ok();
        }
        [HttpPut("unpin/{comment}")]
        public async Task<ActionResult> unpinCommentVideo([FromRoute] int comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CommentVideoDTO ans = await _comService.GetCommentVideoById(comment);
            if (ans == null)
            {
                return NotFound();
            }

            ans.IsPinned = false;

            CommentVideoDTO c = await _comService.UpdateCommentVideo(ans);

            await WebSocketHelper.SendMessageToAllAsync("new_comment", null);

            return Ok();
        }

        [HttpPost("add")]
        public async Task<ActionResult<CommentVideoDTO>> Add([FromBody] CommentVideoDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CommentVideoDTO ans = await _comService.AddCommentVideo(request);

            await WebSocketHelper.SendMessageToAllAsync("new_comment", null);

            return Ok(ans);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CommentVideoDTO>> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CommentVideoDTO ans = await _comService.GetCommentVideoById(id);
            if (ans == null)
            {
                return NotFound();
            }

            await _comService.DeleteCommentVideo(id);

            await WebSocketHelper.SendMessageToAllAsync("new_comment",null);

            return Ok(ans);
        }

        [HttpGet("getbyuserid/{user_id}")]
        public async Task<ActionResult<List<CommentVideoDTO>>> ByUserId([FromRoute] int user_id)
        {

            List<CommentVideoDTO> list = await _comService.GetByUser(user_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }
        [HttpGet("getbyvideoid/{video_id}")]
        public async Task<ActionResult<CommentVideoDTO>> ByVideoId([FromRoute] int video_id)
        {

            List<CommentVideoDTO> list = await _comService.GetCommentsVideoByVideo(video_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }
        [HttpGet("getbyvideowithpagination/{pageNumber}/{pageSize}/{video_id}")]
        public async Task<ActionResult<CommentVideoDTO>> ByVideoIdPaginated([FromRoute] int pageNumber, [FromRoute] int pageSize, [FromRoute] int video_id)
        {

            List<CommentVideoDTO> list = await _comService.GetByVideoPaginated(pageNumber, pageSize, video_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }

        
    }
}
