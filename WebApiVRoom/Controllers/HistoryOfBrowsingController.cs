using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryOfBrowsingController : Controller
    {

        private IHistoryOfBrowsingService _hbService;

        public HistoryOfBrowsingController(IHistoryOfBrowsingService hbService)
        {
            _hbService = hbService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<HistoryOfBrowsingDTO>> GetHistoryOfBrowsing(int id)
        {

            var ans = await _hbService.GetById(id);
            if (ans == null)
            {
                return NotFound();
            }
            return new ObjectResult(ans);
        }
        [HttpPut("update")]
        public async Task<ActionResult<HistoryOfBrowsingDTO>> UpdateHistoryOfBrowsing(HistoryOfBrowsingDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            HistoryOfBrowsingDTO ans = await _hbService.GetById(u.Id);
            if (ans == null)
            {
                return NotFound();
            }

            HistoryOfBrowsingDTO answer = await _hbService.Update(u);

            return Ok(answer);
        }

        [HttpPost("add")]
        public async Task<ActionResult<HistoryOfBrowsingDTO>> Add([FromBody] HistoryOfBrowsingDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HistoryOfBrowsingDTO ans = await _hbService.Add(request);

            return Ok(ans);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<HistoryOfBrowsingDTO>> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            HistoryOfBrowsingDTO ans = await _hbService.GetById(id);
            if (ans == null)
            {
                return NotFound();
            }

            await _hbService.Delete(id);

            return Ok(ans);
        }

        [HttpDelete("deleteall/{clerk_id}")]
        public async Task<ActionResult<HistoryOfBrowsingDTO>> DeleteAll(string clerk_id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<HistoryOfBrowsingDTO> ans = await _hbService.GetByUserId(clerk_id);
            if (ans == null)
            {
                return NotFound();
            }

            foreach (var item in ans)
            {
                await _hbService.Delete(item.Id);
            }
            

            return Ok(ans);
        }

        [HttpGet("getbyuserid/{clerk_id}")]
        public async Task<ActionResult<List<HistoryOfBrowsingDTO>>> ByUserId(string clerk_id)
        {

            List<HistoryOfBrowsingDTO> list = await _hbService.GetByUserId(clerk_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }
        [HttpGet("getallhistorybyclerkidgroupedbydate/{clerk_id}")]
        public async Task<ActionResult<List<HistoryOfBrowsingGroupDateDTO>>> GetAllHistoryByClerkIdGroupedByDate(string clerk_id)
        {

            List<HistoryOfBrowsingGroupDateDTO> list = await _hbService.GetAllHistoryByIdGroupedByDate(clerk_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }
        [HttpGet("getbyuseridpaginated/{pageNumber}/{pageSize}/{clerk_id}")]
        public async Task<ActionResult<List<HistoryOfBrowsingDTO>>> ByUserPaginated([FromRoute] int pageNumber, [FromRoute] int pageSize, [FromRoute] string clerk_id)
        {

            List<HistoryOfBrowsingDTO> list = await _hbService.GetByUserIdPaginated(pageNumber, pageSize,clerk_id);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }
    }
}
