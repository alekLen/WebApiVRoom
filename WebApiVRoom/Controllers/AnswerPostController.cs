using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Entities;
using WebApiVRoom.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerPostController : Controller
    {
        private IAnswerPostService _answerService;
       private ILikesDislikesAPService _likesService;
        private readonly IHubContext<ChatHub> _hubContext;

        public AnswerPostController(IAnswerPostService ansService, ILikesDislikesAPService likesService, IHubContext<ChatHub> hubContext)
        {
            _answerService = ansService;
            _likesService = likesService;
            _hubContext= hubContext;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerPostDTO>> GetAnswer(int id)
        {

            var ans = await _answerService.GetById(id);
            if (ans == null)
            {
                return NotFound();
            }
            return new ObjectResult(ans);
        }
        [HttpPut("update")]
        public async Task<ActionResult<AnswerPostDTO>> UpdateAnswer(AnswerPostDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AnswerPostDTO ans = await _answerService.GetById(u.Id);
            if (ans == null)
            {
                return NotFound();
            }

            AnswerPostDTO answer = await _answerService.Update(u);

            object obj = ConvertObject(answer);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "update_answerpost", payload = obj });


            return Ok(answer);
        }

        [HttpPost("add")]
        public async Task<ActionResult<AnswerPostDTO>> Add([FromBody] AnswerPostDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AnswerPostDTO ans = await _answerService.Add(request);
            object obj = ConvertObject(ans);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "new_answerpost", payload = obj });

            return Ok(ans);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AnswerPostDTO>> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AnswerPostDTO ans = await _answerService.GetById(id);
            if (ans == null)
            {
                return NotFound();
            }

            await _answerService.Delete(id);
            object obj = ConvertObject(ans);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "delete_answerpost", payload = obj });

            return Ok(ans);
        }

        [HttpGet("getbyuserid/{user_id}")]
        public async Task<ActionResult<AnswerPostDTO>> ByUserId(int user_id)
        {

            AnswerPostDTO answer = await _answerService.GetByUser(user_id);
            if (answer == null)
            {
                return NotFound();
            }
            return new ObjectResult(answer);
        }
        [HttpGet("getbycommentid/{com_id}")]
        public async Task<ActionResult<List<AnswerPostDTO>>> ByCommentId(int com_id)
        {

           var answer1 = await _answerService.GetByComment(com_id);
            List<AnswerPostDTO> answer=answer1.ToList();
            if (answer == null)
            {
                return NotFound();
            }
            return new ObjectResult(answer);
        }

        [HttpPut("like/{answer}/{user}/{i}")]
        public async Task<ActionResult> likeAnswerPost([FromRoute] int answer, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesAPDTO like = await _likesService.Get(answer, user);
            if (like == null && user != i)
            {
                LikesDislikesAPDTO likeDto = new() { answerId = answer, userId = user };
                await _likesService.Add(likeDto);
                AnswerPostDTO ans = await _answerService.GetById(answer);
                if (ans == null)
                {
                    return NotFound();
                }
                ans.LikeCount += 1;

                AnswerPostDTO c = await _answerService.Update(ans);
                object obj = ConvertObject(c);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "like_answerpost", payload = obj });

                return Ok();
            }

            return Ok();
        }
        [HttpPut("dislike/{answer}/{user}/{i}")]
        public async Task<ActionResult> dislikeAnswerPost([FromRoute] int answer, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesAPDTO like = await _likesService.Get(answer, user);
            if (like == null && user != i)
            {
                LikesDislikesAPDTO likeDto = new() { answerId = answer, userId = user };
                await _likesService.Add(likeDto);
                AnswerPostDTO ans = await _answerService.GetById(answer);
                if (ans == null)
                {
                    return NotFound();
                }
                ans.DislikeCount += 1;

                AnswerPostDTO c = await _answerService.Update(ans);
                object obj = ConvertObject(c);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "dislike_answerpost", payload = obj });

                return Ok();
            }
            return Ok();
        }

        private object ConvertObject(AnswerPostDTO ans)
        {
            object obj = new
            {
                id = ans.Id,
                userId = ans.UserId,
                userName = ans.UserName,
                channelBanner = ans.ChannelBanner,
                commentPost_Id = ans.CommentPost_Id,
                text = ans.Text,
                answerDate = ans.AnswerDate,
                likeCount = ans.LikeCount,
                dislikeCount = ans.DislikeCount,
                isEdited = ans.IsEdited,
            };
            return obj;
        }
    }
}
