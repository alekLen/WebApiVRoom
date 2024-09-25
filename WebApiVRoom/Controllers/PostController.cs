using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;
using WebApiVRoom.BLL.Services;
using WebApiVRoom.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiVRoom.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
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
            var post = await _postService.GetPost(channel_id);
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

            await _postService.AddPost(img,video,req.text,req.id);
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

            return Ok(post);
        }
    }
}
