using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayListController : Controller
    {
        private IPlayListService _plService;

        public PlayListController(IPlayListService pService)
        {
            _plService = pService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<PlayListDTO>> GetPlayList(int id)
        {

            var plist = await _plService.GetById(id);
            if (plist == null)
            {
                return NotFound();
            }
            return new ObjectResult(plist);
        }
        [HttpPut("update")]
        public async Task<ActionResult<PlayListDTO>> UpdatePlayList(PlayListDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            PlayListDTO ans = await _plService.GetById(u.Id);
            if (ans == null)
            {
                return NotFound();
            }

            PlayListDTO plist = await _plService.Update(u);

            return Ok(plist);
        }

        [HttpPost("add")]
        public async Task<ActionResult<PlayListDTO>> Add([FromBody] PlayListDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PlayListDTO ans = await _plService.Add(request);

            return Ok(ans);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PlayListDTO>> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PlayListDTO ans = await _plService.GetById(id);
            if (ans == null)
            {
                return NotFound();
            }

            await _plService.Delete(id);

            return Ok(ans);
        }
        [HttpDelete("deleterangeplaylist")]
        public async Task<ActionResult> DeleteRangePlaylist([FromBody] List<int> playlistIdsToDelete)
        {
            bool notFoundIds = false;

            if (playlistIdsToDelete == null || !playlistIdsToDelete.Any())
            {
                return NotFound("Список ID пустой.");
            }

            foreach (var id in playlistIdsToDelete)
            {

                PlayListDTO ans = await _plService.GetById(id);
                if (ans == null)
                {
                    notFoundIds = true;
                }

                await _plService.Delete(id);
            }

            if (notFoundIds)
            {
                return Ok(new
                {
                    Message = "Некоторые плей листы не найдены и были пропущены."
                });
            }

            return NoContent();
        }
        [HttpGet("getbyuserid/{clerk_id}")]
        public async Task<ActionResult<List<PlayListDTO>>> ByUserId([FromRoute]string clerk_id)//GetPlayListsByClerkUserId
        {
            List<PlayListDTO> pl = await _plService.GetByUser(clerk_id);
            if (pl == null)
            {
                return NotFound();
            }
            return new ObjectResult(pl);
        }
        [HttpGet("getbyuseridpaginated/{pageNumber}/{pageSize}/{clerk_id}")]
        public async Task<ActionResult<List<PlayListDTO>>> ByUserPaginated([FromRoute] int pageNumber, [FromRoute] int pageSize, [FromRoute] string clerk_id)
        {

            List<PlayListDTO> list = await _plService.GetByUserPaginated(pageNumber, pageSize, clerk_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }
    }
}
