﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;
using System.Text;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerVideoController : Controller
    {
        private IAnswerVideoService _answerService;
        private ILikesDislikesAVService _likesService;
        private readonly IHubContext<ChatHub> _hubContext;

        public AnswerVideoController(IAnswerVideoService ansService, ILikesDislikesAVService likesService, IHubContext<ChatHub> hubContext)
        {
            _answerService = ansService;
            _likesService = likesService;
            _hubContext = hubContext;
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
            object obj=ConvertObject(answer);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "update_answer", payload = obj });

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

            object obj = ConvertObject(ans);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "new_answer", payload = obj });

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
            object obj = ConvertObject(ans);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "delete_answer", payload = obj });

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

            var answer1 = await _answerService.GetByComment(com_id);
            List<AnswerVideoDTO> answer =answer1.ToList();
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
                object obj = ConvertObject(c);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "like_answer", payload = obj });

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
                object obj = ConvertObject(c);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", new { type = "dislike_answer", payload = obj });

                return Ok();
            }
            return Ok();
        }

        private object ConvertObject(AnswerVideoDTO ans)
        {
            object obj = new
            {
                id = ans.Id,
                userId = ans.UserId,
                userName = ans.UserName,
                channelBanner = ans.ChannelBanner,
                commentVideo_Id = ans.CommentVideo_Id,
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
