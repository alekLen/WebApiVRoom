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
    public class SubscriptionController : ControllerBase
    {
        private ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionDTO>>> GetSubscriptions()
        {
            return new ObjectResult(await _subscriptionService.GetAllSubscriptions());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionDTO>> GetSubscription(int id)
        {
            var subscription = await _subscriptionService.GetSubscription(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return new ObjectResult(subscription);
        }

        [HttpGet("getbysubscription/{subscription_id}")]
        public async Task<ActionResult<SubscriptionDTO>> GetSubscriptionsByChannellId(int subscription_id)
        {
            var subscription = await _subscriptionService.GetSubscriptionsByChannelId(subscription_id);
            if (subscription == null)
            {
                return NotFound();
            }
            return new ObjectResult(subscription);
        }

        [HttpGet("getbyuserid/{user_id}")]
        public async Task<ActionResult<SubscriptionDTO>> GetSubscriptionsByUserId(int user_id)
        {
            var subscription = await _subscriptionService.GetSubscriptionsByUserId(user_id);
            if (subscription == null)
            {
                return NotFound();
            }
            return new ObjectResult(subscription);
        }

        [HttpPost("add")]
        public async Task<ActionResult<SubscriptionDTO>> AddSubscription(SubscriptionDTO subscriptionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _subscriptionService.AddSubscription(subscriptionDTO);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<ActionResult<SubscriptionDTO>> UpdateSubscription(SubscriptionDTO u)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SubscriptionDTO subscription = await _subscriptionService.GetSubscription(u.Id);
            if (subscription == null)
            {
                return NotFound();
            }

            SubscriptionDTO subscription_new = await _subscriptionService.UpdateSubscription(u);

            return Ok(subscription_new);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SubscriptionDTO>> DeleteSubscription(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SubscriptionDTO subscription = await _subscriptionService.GetSubscription(id);
            if (subscription == null)
            {
                return NotFound();
            }

            await _subscriptionService.DeleteSubscription(id);

            return Ok(subscription);
        }
    }
}