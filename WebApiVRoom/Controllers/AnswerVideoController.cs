using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerVideoController : Controller
    {
        private IAnswerVideoService _answerService;

        public AnswerVideoController(IAnswerVideoService ansService)
        {
            _answerService = ansService;
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
    }
}
