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
    public class TagController : ControllerBase
    {
        private ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetTags()
        {
            var tags = await _tagService.GetAllTags();
            List<string> tagsList = new List<string>();
            foreach (var tag in tags) { 
                tagsList.Add(tag.Name);
            }
            return new ObjectResult(tagsList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetTag(int id)
        {
            var tag = await _tagService.GetTag(id);
            if (tag == null)
            {
                return NotFound();
            }
            return new ObjectResult(tag);
        }

        [HttpGet("getbytagname/{tagName}")]
        public async Task<ActionResult<TagDTO>> GetByPostText(string tagName)
        {
            var tag = await _tagService.GetTagByName(tagName);
            if (tag == null)
            {
                return NotFound();
            }
            return new ObjectResult(tag);
        }

       
        [HttpPost("add")]
        public async Task<ActionResult<TagDTO>> AddTag(TagDTO tagDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tagService.AddTag(tagDTO);
            return Ok();
        }



        // GET: CategoryController/Edit/5
        [HttpPut("update")]
        public async Task<ActionResult<TagDTO>> UpdateTag(TagDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            TagDTO tag = await _tagService.GetTag(u.Id);
            if (tag == null)
            {
                return NotFound();
            }

            TagDTO tag_new = await _tagService.UpdateTag(u);

            return Ok(tag_new);
        }

        // GET: CategoryController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TagDTO>> DeleteTag(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TagDTO tag = await _tagService.GetTag(id);
            if (tag == null)
            {
                return NotFound();
            }

            await _tagService.DeleteTag(id);

            return Ok(tag);
        }
    }
}
