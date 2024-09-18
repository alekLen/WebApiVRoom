using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerVideoController : Controller
    {
        private IAnswerVideoService _answerService;
        private ILikesDislikesAVService _likesService;

        public AnswerVideoController(IAnswerVideoService ansService, ILikesDislikesAVService likesService)
        {
            _answerService = ansService;
            _likesService = likesService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerVideoDTO>> GetAnswer(int id)
        {

            var ans = await _answerService.GetById(id);
            if (ans == null)
            {
                return NotFound();
            }
            return new ObjectResult(ans);
        }
        [HttpPut("update")]
        public async Task<ActionResult<AnswerVideoDTO>> UpdateAnswer(AnswerVideoDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AnswerVideoDTO ans = await _answerService.GetById(u.Id);
            if (ans == null)
            {
                return NotFound();
            }

            AnswerVideoDTO answer = await _answerService.Update(u);

            await SendMessage();

            return Ok(answer);
        }

        [HttpPost("add")]
        public async Task<ActionResult<AnswerVideoDTO>> Add([FromBody] AnswerVideoDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AnswerVideoDTO ans = await _answerService.Add(request);

            await SendMessage();

            return Ok(ans);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AnswerVideoDTO>> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AnswerVideoDTO ans = await _answerService.GetById(id);
            if (ans == null)
            {
                return NotFound();
            }

            await _answerService.Delete(id);

            await SendMessage();

            return Ok(ans);
        }

        [HttpGet("getbyuserid/{user_id}")]
        public async Task<ActionResult<AnswerVideoDTO>> ByUserId(int user_id)
        {

            AnswerVideoDTO answer = await _answerService.GetByUser(user_id);
            if (answer == null)
            {
                return NotFound();
            }
            return new ObjectResult(answer);
        }
        [HttpGet("getbycommentid/{com_id}")]
        public async Task<ActionResult<AnswerVideoDTO>> ByCommentId(int com_id)
        {

            AnswerVideoDTO answer = await _answerService.GetByComment(com_id);
            if (answer == null)
            {
                return NotFound();
            }
            return new ObjectResult(answer);
        }

        [HttpPut("like/{answer}/{user}/{i}")]
        public async Task<ActionResult> likeAnswerVideo([FromRoute] int answer, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesAVDTO like = await _likesService.Get(answer, user);
            if (like == null && user != i)
            {
                LikesDislikesAVDTO likeDto = new() { answerId = answer, userId = user };
                await _likesService.Add(likeDto);
                AnswerVideoDTO ans = await _answerService.GetById(answer);
                if (ans == null)
                {
                    return NotFound();
                }
                ans.LikeCount += 1;

                AnswerVideoDTO c = await _answerService.Update(ans);

                await SendMessage();

                return Ok();
            }

            return Ok();
        }
        [HttpPut("dislike/{answer}/{user}/{i}")]
        public async Task<ActionResult> dislikeAnswerVideo([FromRoute] int answer, [FromRoute] string user, [FromRoute] string i)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LikesDislikesAVDTO like = await _likesService.Get(answer, user);
            if (like == null && user != i)
            {
                LikesDislikesAVDTO likeDto = new() { answerId = answer, userId = user };
                await _likesService.Add(likeDto);
                AnswerVideoDTO ans = await _answerService.GetById(answer);
                if (ans == null)
                {
                    return NotFound();
                }
                ans.DislikeCount += 1;

                AnswerVideoDTO c = await _answerService.Update(ans);

                await SendMessage();

                return Ok();
            }
            return Ok();
        }

        private async Task SendMessage()
        {
            var message = new
            {
                type = "new_comment"
            };

            var messageJson = System.Text.Json.JsonSerializer.Serialize(message);

            // Отправляем сообщение всем активным WebSocket-клиентам
            foreach (var socket in WebSocketConnectionManager.GetAllSockets())
            {
                if (socket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(messageJson);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
