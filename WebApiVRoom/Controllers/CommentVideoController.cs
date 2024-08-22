using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentVideoController : Controller
    {
        private ICommentVideoService _comService;

        public CommentVideoController(ICommentVideoService cService)
        {
            _comService = cService;
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

            return Ok(c);
        }

        [HttpPost("add")]
        public async Task<ActionResult<CommentVideoDTO>> Add([FromBody] CommentVideoDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CommentVideoDTO ans = await _comService.AddCommentVideo(request);

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
