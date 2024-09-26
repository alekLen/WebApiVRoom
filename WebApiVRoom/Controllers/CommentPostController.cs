using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentPostController : Controller
    {
        private ICommentPostService _comService;
        ILikesDislikesCPService _likesService;

        public CommentPostController(ICommentPostService cService, ILikesDislikesCPService likesService)
        {
            _comService = cService;
            _likesService = likesService;
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
    }
}
