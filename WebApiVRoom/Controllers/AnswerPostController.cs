using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Entities;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerPostController : Controller
    {
        private IAnswerPostService _answerService;

        public AnswerPostController(IAnswerPostService ansService)
        {
            _answerService = ansService;
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
        public async Task<ActionResult<AnswerPostDTO>> ByCommentId(int com_id)
        {

            AnswerPostDTO answer = await _answerService.GetByComment(com_id);
            if (answer == null)
            {
                return NotFound();
            }
            return new ObjectResult(answer);
        }
    }
}
