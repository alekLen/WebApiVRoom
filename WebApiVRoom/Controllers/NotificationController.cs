﻿using Microsoft.AspNetCore.Mvc;
using WebApiVRoom.BLL.DTO;
using WebApiVRoom.BLL.Interfaces;

namespace WebApiVRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private INotificationService _nService;

        public NotificationController(INotificationService nService)
        {
            _nService = nService;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDTO>> GetNotification(int id)
        {

            var nf = await _nService.GetById(id);
            if (nf == null)
            {
                return NotFound();
            }
            return new ObjectResult(nf);
        }
        [HttpPut("markasread/{id}")]
        public async Task<ActionResult<NotificationDTO>> ReadNotification(int id)
        {

            var nf = await _nService.GetById(id);
            if (nf == null)
            {
                return NotFound();
            }
            nf.IsRead = true;
            var nf2= await _nService.Update(nf);
            return new ObjectResult(nf2);
        }
        [HttpPut("markallasread/{userid}")]
        public async Task<ActionResult<NotificationDTO>> ReadAllNotification(string userid)
        {

            List<NotificationDTO> nf = await _nService.GetByUser(userid);
            foreach (NotificationDTO nf2 in nf)
            {
                nf2.IsRead = true;
                await _nService.Update(nf2);
            }
            return Ok();
        }
        [HttpPut("update")]
        public async Task<ActionResult<NotificationDTO>> UpdateNotification(NotificationDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            NotificationDTO nf = await _nService.GetById(u.Id);
            if (nf == null)
            {
                return NotFound();
            }

            NotificationDTO nn = await _nService.Update(u);

            return Ok(nn);
        }

        [HttpPost("add")]
        public async Task<ActionResult<NotificationDTO>> Add([FromBody] NotificationDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            NotificationDTO nf = await _nService.Add(request);

            return Ok(nf);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NotificationDTO>> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            NotificationDTO ns = await _nService.GetById(id);
            if (ns == null)
            {
                return NotFound();
            }

            await _nService.Delete(id);

            return Ok(ns);
        }

        [HttpGet("getbyuserid/{clerk_id}")]
        public async Task<ActionResult<List<NotificationDTO>>> ByUserId(string clerk_id)
        {

            try
            {
                List<NotificationDTO> nf = await _nService.GetByUser(clerk_id);
                if (nf == null)
                {
                    return NotFound();
                }
                return Ok(nf);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        [HttpGet("getbyuserid/{pageNumber}/{pageSize}/{clerk_id}")]
        public async Task<ActionResult<List<NotificationDTO>>> ByUserIdPaginated([FromRoute] int pageNumber, [FromRoute] int pageSize, [FromRoute] string clerk_id)
        {

            try
            {
                List<NotificationDTO> nf = await _nService.GetByUserPaginated(pageNumber,pageSize,clerk_id);
                if (nf == null)
                {
                    return NotFound();
                }
                return Ok(nf);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
        [HttpGet("getbydate")]
        public async Task<ActionResult<List<NotificationDTO>>> ByDate(string date)
        {
            if (!DateTime.TryParse(date, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format. Please provide the date in a valid format.");
            }
            List<NotificationDTO> list = await _nService.GetByDate(parsedDate);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }

        [HttpGet("getbydaterange")]
        public async Task<ActionResult<List<NotificationDTO>>> ByDateRange(string startDate,string endDate)
        {
            if (!DateTime.TryParse(startDate, out DateTime parsedStartDate) || !DateTime.TryParse(endDate, out DateTime parsedEndDate))
            {
                return BadRequest("Invalid date format. Please provide the date in a valid format.");
            }
            if (parsedStartDate > parsedEndDate)
            {
                return BadRequest("Start date cannot be greater than end date.");
            }
            List<NotificationDTO> list = await _nService.GetByDateRange(parsedStartDate, parsedEndDate);
            if (list == null)
            {
                return NotFound();
            }
            return new ObjectResult(list);
        }
    }
}
